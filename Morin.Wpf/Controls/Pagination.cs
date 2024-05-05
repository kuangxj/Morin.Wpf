using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Morin.Wpf.Controls;

[TemplatePart(Name = PageSizeTextBoxTemplateName, Type = typeof(TextBox))]
[TemplatePart(Name = TargetPageTextBoxTemplateName, Type = typeof(TextBox))]
[TemplatePart(Name = ListBoxTemplateName, Type = typeof(ListBox))]
public class Pagination : Control
{
    private const string PageSizeTextBoxTemplateName = "PART_PageSizeTextBox";
    private const string TargetPageTextBoxTemplateName = "PART_TargetPageTextBox";
    private const string ListBoxTemplateName = "PART_ListBox";

    public static RoutedCommand? PrevCommand { get; private set; }
    public static RoutedCommand? NextCommand { get; private set; }

    private const string Ellipsis = "···";
    private static readonly Type? _typeofSelf = typeof(Pagination);

    private TextBox? _textBoxPageSize;
    private TextBox? _textBoxTargetPage;
    private ListBox? _listBoxPages;

    static Pagination()
    {
        InitializeCommands();

        DefaultStyleKeyProperty.OverrideMetadata(_typeofSelf, new FrameworkPropertyMetadata(_typeofSelf));
    }

    #region Override

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        UnsubscribeEvents();

        if (GetTemplateChild(PageSizeTextBoxTemplateName) is TextBox textPageSize)
        {
            _textBoxPageSize = textPageSize;
            _textBoxPageSize.ContextMenu = null;
            _textBoxPageSize.PreviewTextInput += textBoxPageSize_PreviewTextInput;
            _textBoxPageSize.PreviewKeyDown += textBoxPageSize_PreviewKeyDown;
        }
        if (GetTemplateChild(TargetPageTextBoxTemplateName) is TextBox textTargetPage)
        {
            _textBoxTargetPage = textTargetPage;
            _textBoxTargetPage.ContextMenu = null;
            _textBoxTargetPage.PreviewTextInput += textBoxPageSize_PreviewTextInput;
            _textBoxTargetPage.PreviewKeyDown += textBoxPageSize_PreviewKeyDown;
        }
        if (GetTemplateChild(ListBoxTemplateName) is ListBox listPages)
        {
            _listBoxPages = listPages;
        }

        Init();

        SubscribeEvents();
    }

    private void textBoxPageSize_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Key.Space == e.Key || Key.V == e.Key && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            e.Handled = true;
        }
    }

    private void textBoxPageSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = int.TryParse(e.Text, out var result);
    }

    #endregion

    #region Command

    private static void InitializeCommands()
    {
        PrevCommand = new RoutedCommand("Prev", _typeofSelf);
        NextCommand = new RoutedCommand("Next", _typeofSelf);

        CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(PrevCommand, OnPrevCommand, OnCanPrevCommand));
        CommandManager.RegisterClassCommandBinding(_typeofSelf, new CommandBinding(NextCommand, OnNextCommand, OnCanNextCommand));
    }

    private static void OnPrevCommand(object sender, RoutedEventArgs e)
    {
        if (sender is Pagination ctrl)
        {
            ctrl.PageIndex--;
        }
    }

    private static void OnCanPrevCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        if (sender is Pagination ctrl)
        {
            e.CanExecute = ctrl.PageIndex > 1;
        }
    }

    private static void OnNextCommand(object sender, RoutedEventArgs e)
    {
        if (sender is Pagination ctrl)
        {
            ctrl.PageIndex++;
        }
    }

    private static void OnCanNextCommand(object sender, CanExecuteRoutedEventArgs e)
    {
        if (sender is Pagination ctrl)
        {
            e.CanExecute = ctrl.PageIndex < ctrl.PageCount;
        }
    }

    #endregion

    #region Properties

    private static readonly DependencyPropertyKey PagesPropertyKey = DependencyProperty.RegisterReadOnly("Pages", typeof(IEnumerable<string>), _typeofSelf, new PropertyMetadata(null));
    public static readonly DependencyProperty PagesProperty = PagesPropertyKey.DependencyProperty;

    public IEnumerable<string> Pages => (IEnumerable<string>)GetValue(PagesProperty);
    private static readonly DependencyPropertyKey PageCountPropertyKey = DependencyProperty.RegisterReadOnly("PageCount", typeof(int), _typeofSelf, new PropertyMetadata(1, OnPageCountPropertyChanged));

    public static readonly DependencyProperty PageCountProperty = PageCountPropertyKey.DependencyProperty;

    public int PageCount => (int)GetValue(PageCountProperty);

    private static void OnPageCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var pageCount = (int)e.NewValue;
    }

    public static readonly DependencyProperty IsLiteProperty = DependencyProperty.Register("IsLite", typeof(bool), _typeofSelf, new PropertyMetadata(false));

    public bool IsLite
    {
        get => (bool)GetValue(IsLiteProperty);
        set => SetValue(IsLiteProperty, value);
    }

    public static readonly DependencyProperty TotalProperty = DependencyProperty.Register("Total", typeof(int), _typeofSelf, new PropertyMetadata(0, OnTotalPropertyChanged, OnTotalCallback));

    public int Total
    {
        get => (int)GetValue(TotalProperty);
        set => SetValue(TotalProperty, value);
    }

    private static object OnTotalCallback(DependencyObject d, object value)
    {
        return Math.Max((int)value, 0);
    }

    private static void OnTotalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination ctrl && e.NewValue is int total)
        {
            ctrl.SetValue(PageCountPropertyKey, (int)Math.Ceiling(total * 1.0 / ctrl.PageSize));
            ctrl.UpdatePages();
        }
    }

    public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), _typeofSelf, new PropertyMetadata(50, OnPageSizePropertyChanged, OnPageSizeCallback));

    public int PageSize
    {
        get => (int)GetValue(PageSizeProperty);
        set => SetValue(PageSizeProperty, value);
    }

    private static object OnPageSizeCallback(DependencyObject d, object value)
    {
        if (value is int countPerPage)
        {
            return Math.Max(countPerPage, 1);
        }
        return value;
    }

    private static void OnPageSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination ctrl && e.NewValue is int countPerPage)
        {
            if (ctrl._textBoxPageSize != null)
            {
                ctrl._textBoxPageSize.Text = countPerPage.ToString();
            }
            ctrl.SetValue(PageCountPropertyKey, (int)Math.Ceiling(ctrl.Total * 1.0 / countPerPage));

            if (ctrl.PageIndex <= 0)
            {
                ctrl.PageIndex = 1;
            }
            else
            {
                ctrl.UpdatePages();
            }
        }
    }

    public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), _typeofSelf, new PropertyMetadata(1, OnCurrentIndexPropertyChanged, OnCurrentIndexCallback));

    public int PageIndex
    {
        get => (int)GetValue(PageIndexProperty);
        set => SetValue(PageIndexProperty, value);
    }

    private static object OnCurrentIndexCallback(DependencyObject d, object value)
    {
        if (value is int current)
        {
            return Math.Max(current, 1);
        }
        return value;
    }

    private static void OnCurrentIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Pagination ctrl && e.NewValue is int current)
        {
            if (ctrl._listBoxPages != null)
            {
                ctrl._listBoxPages.SelectedItem = current;
            }
            if (ctrl._textBoxTargetPage != null)
            {
                ctrl._textBoxTargetPage.Text = current.ToString();
            }
            ctrl.UpdatePages();
        }
    }

    #endregion

    #region Event

    /// <summary>
    ///     分页
    /// </summary>
    private void OnPageSizeTextBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (_textBoxPageSize != null && sender is TextBox textBox)
        {
            if (int.TryParse(_textBoxPageSize.Text, out var _ountPerPage))
            {
                PageSize = _ountPerPage;
            }
        }
    }

    /// <summary>
    ///     跳转页
    /// </summary>
    private void OnTargetPageTextBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out var _current))
            {
                PageIndex = _current;
            }
        }
    }

    /// <summary>
    ///     选择页
    /// </summary>
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem != null)
        {
            if (int.TryParse(listBox.SelectedItem.ToString(), out var _pageindex))
            {
                PageIndex = _pageindex;
            }
        }
    }

    #endregion

    #region Private

    private void Init()
    {
        SetValue(PageCountPropertyKey, (int)Math.Ceiling(Total * 1.0 / PageSize));
        if (_textBoxTargetPage != null)
        {
            _textBoxTargetPage.Text = PageIndex.ToString();
        }
        if (_textBoxPageSize != null)
        {
            _textBoxPageSize.Text = PageSize.ToString();
        }

        if (_listBoxPages != null)
        {
            _listBoxPages.SelectedItem = PageIndex.ToString();
        }
    }

    private void UnsubscribeEvents()
    {
        if (_textBoxPageSize != null)
        {
            _textBoxPageSize.TextChanged -= OnPageSizeTextBoxChanged;
        }
        if (_textBoxTargetPage != null)
        {
            _textBoxTargetPage.TextChanged -= OnTargetPageTextBoxChanged;
        }
        if (_listBoxPages != null)
        {
            _listBoxPages.SelectionChanged -= OnSelectionChanged;
        }
    }

    private void SubscribeEvents()
    {
        if (_textBoxPageSize != null)
        {
            _textBoxPageSize.TextChanged += OnPageSizeTextBoxChanged;
        }
        if (_textBoxTargetPage != null)
        {
            _textBoxTargetPage.TextChanged += OnTargetPageTextBoxChanged;
        }
        if (_listBoxPages != null)
        {
            _listBoxPages.SelectionChanged += OnSelectionChanged;
        }
    }

    private void UpdatePages()
    {
        SetValue(PagesPropertyKey, GetPagers(Total, PageIndex));

        if (_listBoxPages != null && _listBoxPages.SelectedItem == null)
        {
            _listBoxPages.SelectedItem = PageIndex.ToString();
        }
    }

    private IEnumerable<string> GetPagers(int count, int current)
    {
        if (count == 0)
        {
            return [];
        }
        if (PageSize <= 7)
        {
            return Enumerable.Range(1, PageSize).Select(p => p.ToString()).ToArray();
        }
        if (current <= 4)
        {
            return new[] { "1", "2", "3", "4", "5", Ellipsis, PageSize.ToString() };
        }
        if (current >= PageSize - 3)
        {
            return new[]
            {
                "1", Ellipsis, (PageSize - 4).ToString(), (PageSize - 3).ToString(), (PageSize - 2).ToString(),
                (PageSize - 1).ToString(), PageSize.ToString()
            };
        }
        return new[]
        {
            "1", Ellipsis, (current - 1).ToString(), current.ToString(), (current + 1).ToString(), Ellipsis,PageSize.ToString()
        };
    }

    #endregion
}
