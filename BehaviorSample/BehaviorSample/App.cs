using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace BehaviorSample {
    public class App : Application {
        public App() {
            MainPage = new MyPage();
        }

        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }

    class MyPage : ContentPage {
        public MyPage() {
            var emailBehavior = new EmailBehavior();

            var button = new Button{
                Text = "Send",
                HorizontalOptions = LayoutOptions.Center,
                IsEnabled = emailBehavior.IsValid
            };

            var entry = new Entry {
                WidthRequest = 200,
                Placeholder = "user@example.com",
                HorizontalOptions = LayoutOptions.Center,
                Behaviors = { emailBehavior }//ビヘイビアの追加
            };
            entry.TextChanged += (sender, args) =>{
                button.IsEnabled = emailBehavior.IsValid; //ビヘイビアクラスの検証結果を使用してボタンを状態を変更する
            };

            Content = new StackLayout {
                Padding = new Thickness(0, Device.OnPlatform(40, 20, 20), 0, 0),
                Children = {entry, button}
            };
        }
    }

    public class EmailBehavior : Behavior<Entry> {
        private readonly Regex _regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        //クラス外から検証結果が分かるようにプロパティを追加
        public bool IsValid { get; private set; }

        protected override void OnAttachedTo(Entry bindable) {
            //ハンドラの追加
            bindable.TextChanged += CheckEmail;
        }

        protected override void OnDetachingFrom(Entry bindable) {
            //ハンドラの削除
            bindable.TextChanged -= CheckEmail;
        }

        private void CheckEmail(object sender, TextChangedEventArgs e) {

            //文字列がEmailとして正しいかどうかのチェック
            var m = _regex.Match(e.NewTextValue);
            //正かどうかによってテキストの色を変化させる
            ((Entry)sender).TextColor = m.Success ? Color.Default : Color.Red;

            IsValid = m.Success; //状態を表すプロパティにセットする


        }
    }
}
