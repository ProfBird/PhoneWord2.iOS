using System;
using System.Drawing;

using Foundation;
using UIKit;
using System.Collections.Generic;

namespace PhoneWord.iOS
{
	public partial class PhoneWord_iOSViewController : UIViewController
	{
		private string translatedNumber = "";

		private List<string> phoneNumbers;	// backing variable

		public List<string> PhoneNumbers
		{
			get { return phoneNumbers; }
			set { phoneNumbers = value; }
		}

		public PhoneWord_iOSViewController (IntPtr handle) : base (handle)
		{
			phoneNumbers = new List<string> ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			


			TranslateButton.TouchUpInside += (object sender, EventArgs e) => {
			
				// Convert the phone number with text to a number 
				// using PhoneTranslator.cs
				translatedNumber = Core.PhonewordTranslator.ToNumber(
					PhoneNumberText.Text);   

				// Dismiss the keyboard if text field was tapped
				PhoneNumberText.ResignFirstResponder ();

				if (translatedNumber == "") {
					CallButton.SetTitle ("Call", UIControlState.Normal);
					CallButton.Enabled = false;
				} 
				else {
					CallButton.SetTitle ("Call " + translatedNumber, UIControlState.Normal);
					CallButton.Enabled = true;
				}
			};


			CallButton.TouchUpInside += (object sender, EventArgs e) => {
				var url = new NSUrl ("tel:" + translatedNumber);

				// Store the phone number for the history screen
				phoneNumbers.Add(translatedNumber);

				// Use URL handler with tel: prefix to invoke Apple's Phone app, 
				// otherwise show an alert dialog                

				if (!UIApplication.SharedApplication.OpenUrl (url)) {
					var av = new UIAlertView ("Not supported",
						"Scheme 'tel:' is not supported on this device",
						null,
						"OK",
						null);
					av.Show ();
				}
			};

			// Load CallHistory view programmatically
			CallHistoryButton.TouchUpInside += (object sender, EventArgs e) =>{
				// Launches a new instance of CallHistoryController
				CallHistoryController callHistory = 
					this.Storyboard.InstantiateViewController ("CallHistoryController") as CallHistoryController;
				if (callHistory != null) {
					callHistory.PhoneNumbers = PhoneNumbers;
					this.NavigationController.PushViewController (callHistory, true);
				}
			};
		}

		// We're not using the Segue any more
//		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
//		{
//			base.PrepareForSegue (segue, sender);
//
//			// set the View Controller that’s powering the screen we’re
//			// transitioning to
//
//			var callHistoryContoller = segue.DestinationViewController as CallHistoryController;
//
//			//set the Table View Controller’s list of phone numbers to the
//			// list of dialed phone numbers
//
//			if (callHistoryContoller != null) {
//				callHistoryContoller.PhoneNumbers = phoneNumbers;
//			}
//		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

