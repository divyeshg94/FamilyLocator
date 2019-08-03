using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Widget;
using FamilyLocator.ChatApp;
using FamilyLocator.Model;
using FamilyLocator.Service;


//Refer https://www.c-sharpcorner.com/article/xamarin-android-sqlite-database/
namespace FamilyLocator
{
    [Activity(Label = "Message")]
    public class MessageActivity : Activity
    {
        ListView lstViewData;
        List<Location> listSource = new List<Location>();
        Database db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.messages);
            var btnSend = FindViewById<Button>(Resource.Id.btnSendMessage);

            var chatApp = new ChatHub();

            btnSend.Click += delegate
            {
                var txtView_message = FindViewById<TextView>(Resource.Id.enterMessage);
                var txtView_viewmessage = FindViewById<TextView>(Resource.Id.viewMessage);
                var txtView_Connection = FindViewById<TextView>(Resource.Id.txtView_Date);
                var message = txtView_message.Text;
                var toConnection = "";//txtView_Connection.Text;
                chatApp.SendMessage(message, toConnection).Wait();
                txtView_viewmessage.Text += message;
            };
        }

        public void OnMessageReceived(string message, string sender)
        {
            var txtView_message = FindViewById<TextView>(Resource.Id.viewMessage);
            txtView_message.Text += sender + ": " + message;
        }
    }
}