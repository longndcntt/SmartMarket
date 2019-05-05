using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartMarket.Views.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingStars : ContentView
    {
        private Label ReviewsLabel { get; set; }
        private List<Image> StarImages { get; set; }

        public RatingStars()
        {
            GenerateDisplay();
        }

        private void GenerateDisplay()
        {

            //Creates Review Count Label 
            ReviewsLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };

            //Create Star Image Placeholders 
            StarImages = new List<Image>();
            for (int i = 0; i < 5; i++)
                StarImages.Add(new Image());

            //Create Horizontal Stack containing stars and review count label 
            StackLayout starsStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                Padding = 0,
                Spacing = 0,
                Children = {
                    StarImages[0],
                    StarImages[1],
                    StarImages[2],
                    StarImages[3],
                    StarImages[4],
                    ReviewsLabel
                }
            };

            updateReviewsDisplay();

            updateStarsDisplay();

            this.Content = starsStack;
        }

        //Set the Display of the Reviews Label 
        public void updateReviewsDisplay()
        {
            ReviewsLabel.Text = Reviews > 0 ? " (" + Convert.ToString(Reviews) + ")" : "";
        }

        //Set the correct images for the stars based on the rating 
        public void updateStarsDisplay()
        {
            for (int i = 0; i < StarImages.Count; i++)
            {
                StarImages[i].Source = GetStarFileName(i);
            }
        }

        //uses zero based position for stars 
        private string GetStarFileName(int position)
        {
            int currentStarMaxRating = position;
            //Rating is out of 10 
            if (Rating > currentStarMaxRating)
            {
                return "rating_star_on.png";
            }
           
            else
            {
                return "rating_star_off.png";
            }
        }

        //Add in configurable "Rating" double property from XAML, for setting the rating stars
        //public static BindableProperty RatingProperty =
        //    BindableProperty.Create<RatingStars, int>(ctrl => ctrl.Rating,
        //        defaultValue: 0,
        //        defaultBindingMode: BindingMode.OneWay,
        //        propertyChanged: (bindable, oldValue, newValue) => {
        //            var ratingStars = (RatingStars)bindable;
        //            ratingStars.updateStarsDisplay();
        //        }
        //    );

        public static readonly BindableProperty RatingProperty =
            BindableProperty.Create(nameof(Rating),
                typeof(int),
                typeof(RatingStars),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ratingStars = (RatingStars)bindable;
                    ratingStars.updateStarsDisplay();
                });

        //Rating is out of 10 
        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        //Add in configurable "Rating" double property from XAML, for setting the rating stars
        //public static BindableProperty ReviewsProperty =
        //    BindableProperty.Create<RatingStars, int>(ctrl => ctrl.Reviews,
        //        defaultValue: 0,
        //        defaultBindingMode: BindingMode.OneWay,
        //        propertyChanged: (bindable, oldValue, newValue) =>
        //        {
        //            var ratingStars = (RatingStars)bindable;
        //            ratingStars.updateReviewsDisplay();
        //        }
        //    );

        public static readonly BindableProperty ReviewsProperty =
            BindableProperty.Create(nameof(Reviews),
                typeof(int),
                typeof(RatingStars),
                0,
                BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ratingStars = (RatingStars)bindable;
                    ratingStars.updateReviewsDisplay();
                });

        //Count of the total number of reviews 
        public int Reviews
        {
            get { return (int)GetValue(ReviewsProperty); }
            set { SetValue(ReviewsProperty, value); }
        }

    }
}