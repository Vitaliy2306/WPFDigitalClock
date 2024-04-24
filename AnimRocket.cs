using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace WpfAppRocket
{
	class AnimRocket
	{
		readonly int numberFrames = 0;

		readonly string[] uriSources = {
			"img/rocket/rocket-main-anim-frame0.png",
			"img/rocket/rocket-main-anim-frame1.png",
			"img/rocket/rocket-main-anim-frame2.png"
		};

		public readonly Size maxImgSize = new(0, 0);

		private readonly Panel? _parent = null;

		public AnimRocket(Panel parent)
		{
			_parent = parent;

			numberFrames = uriSources.Length;

			maxImgSize = ComputeMaxImageSize();


			Animation();
		}


		public void Rotate(double angle)
		{
			if (_parent != null)
			{
				RotateTransform rt = new(angle);
				_parent.Children[0].RenderTransform = rt;
			}
		}


		private void Animation()
		{

			ObjectAnimationUsingKeyFrames objectAnimation = new()
			{
				Duration = new Duration(TimeSpan.FromSeconds(0.03)),
				RepeatBehavior = RepeatBehavior.Forever
			};

			for (int i = 0; i < numberFrames; i++)
			{
				double quota = (double)i / numberFrames;

				objectAnimation.KeyFrames.Add(
					new DiscreteObjectKeyFrame()
					{
						Value = new BitmapImage(new Uri(uriSources[i], UriKind.Relative)),
						KeyTime = KeyTime.FromPercent(quota)
					}
				);
			}

			Image image = new()
			{
				Margin = new Thickness(0, 0, 0, 0),
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Stretch = Stretch.None,
				RenderTransformOrigin = new Point(0.5, 0.5)
			};

			_parent?.Children.Add(image);
			image.BeginAnimation(Image.SourceProperty, objectAnimation);
		}

		/// <summary>
		/// Отримання максимальних розмірів об'єкту
		/// </summary>
		/// <returns></returns>
		private Size ComputeMaxImageSize()
		{
			List<double> maxW = new();
			List<double> maxH = new();

			// Вираховуємо габаритний розмір анімації
			for (int i = 0; i < numberFrames; i++)
			{
				BitmapImage bmp = new(new Uri(uriSources[i], UriKind.Relative));
				maxW.Add(bmp.Width);
				maxH.Add(bmp.Height);
			}


			return new Size(maxW.Max(x => x), maxH.Max(y => y));
		}
	}
}
