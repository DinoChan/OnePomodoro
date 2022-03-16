using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;

namespace OnePomodoro.Helpers
{
    public class MarkdownTextBlockExtensions
    {
        /// <summary>
        /// 从指定元素获取 FileUri 依赖项属性的值。
        /// </summary>
        /// <param name="obj">从中读取属性值的元素。</param>
        /// <returns>从属性存储获取的属性值。</returns>
        public static Uri GetFileUri(MarkdownTextBlock obj) => (Uri)obj.GetValue(FileUriProperty);

        /// <summary>
        /// 将 FileUri 依赖项属性的值设置为指定元素。
        /// </summary>
        /// <param name="obj">对其设置属性值的元素。</param>
        /// <param name="value">要设置的值。</param>
        public static void SetFileUri(DependencyObject obj, Uri value) => obj.SetValue(FileUriProperty, value);

        /// <summary>
        /// 标识 FileUri 依赖项属性。
        /// </summary>
        public static readonly DependencyProperty FileUriProperty =
            DependencyProperty.RegisterAttached("FileUri", typeof(Uri), typeof(MarkdownTextBlockExtensions), new PropertyMetadata(default(Uri), OnFileUriChanged));

        private static void OnFileUriChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (Uri)args.OldValue;
            var newValue = (Uri)args.NewValue;
            if (oldValue == newValue)
                return;

            //var target = obj as MarkdownTextBlock;
            //var uri = new Uri("ms-appx:///Assets/Models/Telescope.gltf");
            //var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            //var text = await FileIO.ReadTextAsync(storageFile);
            //target.Text = text;
        }
    }
}
