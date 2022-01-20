using Xamarin.Forms;

namespace FormsLib
{
    public class LabelCheckBox : StackLayout
    {
        /// <summary>
        /// constructor
        /// </summary>
        public LabelCheckBox(string text)
        {
            Orientation = StackOrientation.Horizontal;
            //Spacing = 4;
            //Padding = 4;
            _label = new Label() { Text = text, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
            _checkBox = new CheckBox() { HorizontalOptions = LayoutOptions.End };
            Children.Add(_checkBox);
            Children.Add(_label);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public LabelCheckBox(string text, double fontSize)
            : this(text)
        {
            _label.FontSize = fontSize;
        }

        /// <summary>
        /// get/set Label text
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value; }
        }

        /// <summary>
        /// get/set CheckBox IsChecked
        /// </summary>
        public bool IsChecked
        {
            get { return _checkBox.IsChecked;}
            set { _checkBox.IsChecked = value; }
        }

        protected CheckBox _checkBox;
        protected Label _label;
    }
}
