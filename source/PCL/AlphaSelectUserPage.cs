﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace keep.grass
{
	public class AlphaSelectUserPage : ResponsiveContentPage
	{
		AlphaDomain Domain = AlphaFactory.MakeSureDomain();
		Languages.AlphaLanguage L = AlphaFactory.MakeSureLanguage();

		public Action<string> Reciever;

		ListView List;
		SearchBar Search;
		ActivityIndicator Indicator;

		static public class ListItem
		{
			public static object Make(GitHub.SearchUser User)
			{
				return new
				{
					//ImageSourceUrl = User.AvatarUrl, 本来こちらのコードであるべきだが、こちらのURLだと他の箇所とキャッシュが分断されてよろしくない。
                    ImageSourceUrl = GitHub.GetIconUrl(User.Login),
					Text = User.Login,
				};
			}
			public static object Make(string User)
			{
				return new
				{
					ImageSourceUrl = GitHub.GetIconUrl(User),
					Text = User,
				};
			}
		}

        public AlphaSelectUserPage(Action<string> aReciever, IEnumerable<string> ExistUsers = null)
		{
			Reciever = aReciever;
			Title = L["Select a user"];

			List = new ListView
			{
				ItemTemplate = new DataTemplateEx(AlphaFactory.GetGitHubUserCellType())
					.SetBindingList("ImageSourceUrl", "Text"),
			};
			List.ItemTapped += (sender, e) =>
			{
                var User = e.Item.GetValue<string>("Text");
                if (null != User)
				{
					Debug.WriteLine($"Select User: {User}");
					Reciever(User);
					Domain.AddRecentUser(User);
				}
				AlphaFactory.MakeSureApp().Navigation.PopAsync();
			};
			List.ItemsSource = Domain.GetRecentUsers()
                .Where(i => !(ExistUsers?.Contains(i) ?? false))
				.Select(i => ListItem.Make(i));

			Search = new SearchBar
			{
				Placeholder = L["User's name etc."],
				SearchCommand = new Command
				(
					async () =>
					{
                        List.IsVisible = false;
                        Indicator.IsVisible = true;
                        Indicator.IsRunning = true;
                        List.ItemsSource = new object[] { };

						try
						{
							var Json = await Domain.GetStringFromUrlAsync
							(
								GitHub.GetSearchUsersUrl(Search.Text)
							);
							Device.BeginInvokeOnMainThread
							(
								() =>
								{
									try
									{
										List.ItemsSource = GitHub.SearchResult<GitHub.SearchUser>.Parse(Json)
										.Items
										.Select(i => ListItem.Make(i));
									}
									catch (Exception Err)
									{
										Debug.WriteLine(Err);
										if (!string.IsNullOrWhiteSpace(Search.Text))
										{
											List.ItemsSource = new[] { Search.Text.Trim() }
												.Select(i => ListItem.Make(i));
										}
									}
									Indicator.IsRunning = false;
									Indicator.IsVisible = false;
									List.IsVisible = true;
								}
							);
						}
						catch (Exception Err)
						{
							Debug.WriteLine(Err);
							if (!string.IsNullOrWhiteSpace(Search.Text))
							{
								List.ItemsSource = new[] { Search.Text.Trim() }
									.Select(i => ListItem.Make(i));
							}
							Indicator.IsRunning = false;
							Indicator.IsVisible = false;
							List.IsVisible = true;
						}
					}
				),
			};

			Indicator = new ActivityIndicator
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				IsVisible = false,
			};

			Content = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				//Spacing = 1.0,
				//BackgroundColor = Color.Gray,
				Children =
				{
					Search,
					List,
					Indicator,
				}
			};
		}
		public override void Build()
		{
			base.Build();
			Debug.WriteLine("AlphaSelectUserPage.Rebuild();");

		}
	}
}

