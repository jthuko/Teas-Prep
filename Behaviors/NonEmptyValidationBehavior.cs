using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.Behaviors
{
    public class NonEmptyValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                bool isValid = !string.IsNullOrEmpty(entry.Text);
                entry.BackgroundColor = isValid ? Color.FromHex("#DFD8F7") : Color.FromHex("#FFCCCC");
                ((Button)entry.FindByName("Login")).IsEnabled = isValid;
                ((Button)entry.FindByName("Signup")).IsEnabled = isValid;
            }
        }
    }
}
