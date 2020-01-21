//using Milestone2.MainWindow.xaml;
using Npgsql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System;
using System.Globalization;
using System.Collections.Generic;

//using System.Windows.Forms;

namespace Milestone2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Business selectedBusiness;
        public Business selectedFavorites;
        public User aUser;
        string selectedUserId;
        string selectedCategoriesnametoAdd;
        string selectedCategoriesnametoRemove;
        public int selectedRating;
        public string sortOption = "ORDER BY name";
        public string new_review_id;
        public string curDate = DateTime.Now.ToString("yyyy-MM-dd");
        public string curDate2 = DateTime.Now.ToString("MM/dd/yyyy");
        public string curTime = DateTime.Now.ToString("HH:mm");
        public string curDay;

        public void getDay(string date)
        {
            DateTime dateValue;
            DateTimeOffset dateOffsetValue;
            // Convert date representation to a date value
            dateValue = DateTime.Parse(curDate2, CultureInfo.InvariantCulture);
            dateOffsetValue = new DateTimeOffset(dateValue, TimeZoneInfo.Local.GetUtcOffset(dateValue));
            this.curDay = dateValue.ToString("dddd");
        }


        string categoriesQuery;
        string[] categoriesArray =  new string[10];
        int index=0, size=0;
        //public User cUser;
        public class Business
        {
            public string business_id { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string zipcode { get; set; }
            public string address { get; set; }
            public string category { get; set; }
            public double stars { get; set; }
            public int review_count { get; set; }
            public double review_ratings { get; set; }
            public int num_checkins { get; set; }
            public double distance { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            //public DateTime hours_open { get; set; }
            //public DateTime hours_close { get; set; }

        }
        public class Review
        {
            public string review_id { get; set; }
            public string review_text { get; set; }
            public string review_stars { get; set; }
            public NpgsqlTypes.NpgsqlDate review_date { get; set; }
            public string Business_id { get; set; }
            public string User_id { get; set; }
            public string user_name { get; set; }
            public string funny { get; set; }
            public string useful { get; set; }
            public string cool { get; set; }
        }

        public class FriendRecentReview
        {
            public string user_name { get; set; }
            public string business_name { get; set; }
            public string city { get; set; }
            public string review_text { get; set; }
        }
        public class User
        {
            public string user_id { get; set; }
            public double average_stars { get; set; }
            public int fans { get; set; }
            public string name { get; set; }
            public int cool { get; set; }
            public int funny { get; set; }
            public int useful { get; set; }
            public int review_count { get; set; }
            public NpgsqlTypes.NpgsqlDate yelping_since { get; set; }
            public double user_latitude { get; set; }
            public double user_longitude { get; set; }

        }
        public class Categories
        {
            public string business_id { get; set; }
            public string Categories_name { get; set; }
        }
        /*
        public class Favorites
        {
            public string business_id { get; set; }
            public string business_name{ get; set; }
            public string business_address { get; set; }
       
            public string user_id { get; set; }
        }*/

        public MainWindow()
        {
            InitializeComponent();
            getDay("04/20/2019");
            addStates();
            addColumns2Grid();
            businessGrid.Items.Clear();
            addIndexRating();
            addIndexSortBy();
        }
        private string buildConnString()
        {
            return "Host = localhost; Username = postgres; password = teamsk; Database = yelpdb;";
        }
        public void addStates()
        {

            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statelist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addCities()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "' ORDER by city;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            citylist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addZipcodes()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct zipcode FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "' AND city ='" + citylist.SelectedItem.ToString() + "' ORDER by zipcode;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            zipcodelist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addCategories()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct categories_name FROM Categories, Business WHERE Business.Business_id = Categories.Business_id AND state = '" + statelist.SelectedItem.ToString() + "' " +
                        "AND city ='" + citylist.SelectedItem.ToString() + "'AND zipcode ='" + zipcodelist.SelectedItem.ToString() + "'  ORDER by categories_name;";


                    //DataTable dt = new DataTable("business");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorylist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addSelectedCategories()
        {
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT distinct categories_name FROM Categories, Business WHERE Business.Business_id = Categories.Business_id AND state = '" + statelist.SelectedItem.ToString() + "' AND city ='" + citylist.SelectedItem.ToString() + "'AND zipcode ='" + zipcodelist.SelectedItem.ToString() + "'  ORDER by categories_name;";

                   //DataTable dt = new DataTable("business");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorylist.Items.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
        }
        public void addIndexRating()
        {
            addRating.Items.Add("5");
            addRating.Items.Add("4");
            addRating.Items.Add("3");
            addRating.Items.Add("2");
            addRating.Items.Add("1");
        }
        public void addIndexSortBy()
        {
            sortByBox.Items.Add("Business Name");
            sortByBox.Items.Add("Highest Ranking");
            sortByBox.Items.Add("Most Reviewed");
            sortByBox.Items.Add("Best Review Ranking");
            sortByBox.Items.Add("Most CheckIn");
            sortByBox.Items.Add("Nearest");
        }


        public void addColumns2Grid()
        {
            //BusinessGrid
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Business Name";
            col1.Binding = new Binding("name");
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();

            col2.Header = "Address";
            col2.Binding = new Binding("address");
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "City";
            col3.Binding = new Binding("city");
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "State";
            col4.Binding = new Binding("state");
            businessGrid.Columns.Add(col4);

            DataGridTextColumn col25 = new DataGridTextColumn();
            col25.Header = "distance(in miles)";
            col25.Binding = new Binding("distance");
            businessGrid.Columns.Add(col25);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Stars";
            col5.Binding = new Binding("stars");
            businessGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "# of review";
            col6.Binding = new Binding("review_count");
            businessGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Review Rating";
            col7.Binding = new Binding("review_ratings");
            businessGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Header = "Total Checkin";
            col8.Binding = new Binding("num_checkins");
            businessGrid.Columns.Add(col8);


            //ReviewGrid
            DataGridTextColumn col9 = new DataGridTextColumn();
            col9.Header = "User Name";
            col9.Binding = new Binding("user_name");
            reviewGrid.Columns.Add(col9);

            DataGridTextColumn col10 = new DataGridTextColumn();
            col10.Header = "Review Date";
            col10.Binding = new Binding("review_date");
            reviewGrid.Columns.Add(col10);

            DataGridTextColumn col11 = new DataGridTextColumn();
            col11.Header = "rate";
            col11.Binding = new Binding("review_stars");
            reviewGrid.Columns.Add(col11);

            DataGridTextColumn col12 = new DataGridTextColumn();
            col12.Header = "Review Text";
            col12.Binding = new Binding("review_text");
            reviewGrid.Columns.Add(col12);


            //FriendsGrid
            DataGridTextColumn col13 = new DataGridTextColumn();
            col13.Header = "Name";
            col13.Binding = new Binding("name");
            friendsGrid.Columns.Add(col13);

            DataGridTextColumn col14 = new DataGridTextColumn();
            col14.Header = "avg_stars";
            col14.Binding = new Binding("average_stars");
            friendsGrid.Columns.Add(col14);

            DataGridTextColumn col15 = new DataGridTextColumn();
            col15.Header = "yelping_since";
            col15.Binding = new Binding("yelping_since");
            friendsGrid.Columns.Add(col15);


            //FavoriteBusinessGrid
            DataGridTextColumn col16 = new DataGridTextColumn();
            col16.Header = "Business Name";
            col16.Binding = new Binding("name");
            favoriteBusinessGrid.Columns.Add(col16);

            DataGridTextColumn col17 = new DataGridTextColumn();
            col17.Header = "stars";
            col17.Binding = new Binding("stars");
            favoriteBusinessGrid.Columns.Add(col17);

            DataGridTextColumn col18 = new DataGridTextColumn();
            col18.Header = "City";
            col18.Binding = new Binding("city");
            favoriteBusinessGrid.Columns.Add(col18);

            DataGridTextColumn col19 = new DataGridTextColumn();
            col19.Header = "Zipcode";
            col19.Binding = new Binding("zipcode");
            favoriteBusinessGrid.Columns.Add(col19);

            DataGridTextColumn col20 = new DataGridTextColumn();
            col20.Header = "Address";
            col20.Binding = new Binding("address");
            favoriteBusinessGrid.Columns.Add(col20);


            //FriendsReviewGrid
            DataGridTextColumn col21 = new DataGridTextColumn();
            col21.Header = "User Name";
            col21.Binding = new Binding("user_name");
            friendsReviewGrid.Columns.Add(col21);

            DataGridTextColumn col22 = new DataGridTextColumn();
            col22.Header = "Businee Name";
            col22.Binding = new Binding("business_name");
            friendsReviewGrid.Columns.Add(col22);

            DataGridTextColumn col23 = new DataGridTextColumn();
            col23.Header = "City";
            col23.Binding = new Binding("city");
            friendsReviewGrid.Columns.Add(col23);

            DataGridTextColumn col24 = new DataGridTextColumn();
            col24.Header = "Text";
            col24.Binding = new Binding("review_text");
            friendsReviewGrid.Columns.Add(col24);
        }
        public void addColumns2ReviewGrid(ReviewsGrid newWindowReview)// Grid colomn for reviews (new window)
        {

            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Header = "Review Date";
            col1.Binding = new Binding("review_date");
            newWindowReview.reviewDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Header = "User Name";
            col2.Binding = new Binding("user_name");
            newWindowReview.reviewDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Header = "Stars";
            col3.Binding = new Binding("review_stars");
            newWindowReview.reviewDataGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Header = "Review Text";
            col4.Binding = new Binding("review_text");
            newWindowReview.reviewDataGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Header = "Funny";
            col5.Binding = new Binding("funny");
            newWindowReview.reviewDataGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Header = "Useful";
            col6.Binding = new Binding("useful");
            newWindowReview.reviewDataGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Header = "Cool";
            col7.Binding = new Binding("cool");
            newWindowReview.reviewDataGrid.Columns.Add(col7);

        }

        private void Statelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            citylist.SelectedValue = -1;
            citylist.Items.Clear();
            zipcodelist.Items.Clear();
            categorylist.Items.Clear();
            reviewGrid.Items.Clear();
            selectedcategorylist.Items.Clear();

            businessCategoriesList.Items.Clear();

            if (statelist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state FROM business WHERE state = '" + statelist.SelectedItem.ToString() + "';";
                    }
                    conn.Close();
                    addCities();

                }

            }
        }

        private void Citylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            zipcodelist.Items.Clear();
            categorylist.Items.Clear();
            reviewGrid.Items.Clear();
            selectedcategorylist.Items.Clear();

            if (citylist.SelectedIndex > -1)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city FROM business WHERE city = '" + citylist.SelectedItem.ToString() + "' AND state = '" + statelist.SelectedItem.ToString() + "' ORDER by name;";
                    }
                    conn.Close();
                    addZipcodes();
                }

            }
        }

        private void Zipcodelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            categorylist.Items.Clear();
            reviewGrid.Items.Clear();
            selectedcategorylist.Items.Clear();
            for (index = 0; index < 10; index++)
                categoriesArray[index] = "";
            size = 0;
            if (zipcodelist.SelectedIndex > -1 && aUser !=null)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT name, state, city, address, " +
                             "(2 * 3961 * asin(sqrt((sin(radians((latitude - " + aUser.user_latitude + ") / 2))) ^ 2 + cos(radians(" + aUser.user_latitude + ")) * cos(radians(latitude)) * (sin(radians((longitude - " + aUser.user_longitude + ") / 2))) ^ 2))) AS distance, " +
                             "stars, review_count, review_ratings, num_checkins FROM business WHERE zipcode = '" + zipcodelist.SelectedItem.ToString() + "' AND city = '" + citylist.SelectedItem.ToString() + "' AND state = '" + statelist.SelectedItem.ToString() + "' " + sortOption + " ;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), address = reader.GetString(3), distance = reader.GetDouble(4), stars = reader.GetDouble(5), review_count = reader.GetInt32(6), review_ratings = reader.GetDouble(7), num_checkins = reader.GetInt32(8) });
                            }
                        }
                    }
                    conn.Close();
                    addCategories();
                }
            }
            else if(aUser == null)
                MessageBox.Show("User must be selected first!");
        }
        private void Categorylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reviewGrid.Items.Clear();

            if (categorylist.SelectedIndex > -1)
            {
                selectedCategoriesnametoAdd = categorylist.SelectedItem.ToString();
            }
        }

        public string randomID()
        {
            char[] letters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm0123456789_-".ToCharArray();
            Random r = new Random();
            string randomreviewid = "";

            for (int i = 0; i < 22; i++)
            {
                randomreviewid += letters[r.Next(0, 63)].ToString();
            }
            return randomreviewid;
        }

        private void addReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            new_review_id = randomID();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    if (aUser != null && selectedBusiness !=null)
                    {
                        cmd.CommandText = "INSERT INTO Review (Review_id, Text, Review_date, Review_stars, Funny, Cool, Useful, Business_id, User_id) " +
                            "VALUES  ('" + new_review_id + "','" + addReviewTxt.Text + "', '" + curDate + "'," + selectedRating + "," + "0,0,0,'" + selectedBusiness.business_id + "','" + aUser.user_id + "');";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                MessageBox.Show("Your Review was saved");
                                while (reader.Read())
                                {
                                    reviewGrid.Items.Add(new Review() { user_name = reader.GetString(0), review_date = reader.GetDate(1), review_stars = reader.GetString(2), review_text = reader.GetString(3) });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        cmd.CommandText = "UPDATE business SET review_ratings = B.avg FROM (SELECT AVG(review_stars) AS avg, business_id FROM review GROUP BY business_id) AS B WHERE business.business_id = B.business_id";
                        cmd.ExecuteReader();
                        
                    }
                    else
                    {
                        MessageBox.Show("User was not selected, or Business was not selected. ");
                    }
                }
                conn.Close();
            }

        }

        private void BusinessGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            businessCategoriesList.Items.Clear();
            reviewGrid.Items.Clear();
            businessOpenInfoTxt.Clear();
            selectedBusiness = (Business)businessGrid.SelectedItem;
            if (selectedBusiness != null)
            {
                businessName.Text = selectedBusiness.name;
                businessAddress.Text = selectedBusiness.address;
               
                using (var conn = new NpgsqlConnection(buildConnString())) //bring all categories form the selected business on the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct Categories_name, Business.business_id FROM Categories, Business " +
                            "WHERE Business.Business_id = Categories.Business_id AND name = '" + selectedBusiness.name.ToString() + "'AND address = '" + selectedBusiness.address.ToString() + "';";

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListViewItem Lv = new ListViewItem();
                                Lv.Content = reader[0].ToString();
                                businessCategoriesList.Items.Add(Lv);
                                selectedBusiness.business_id = reader[1].ToString();
                            }
                        }
                    }
                    conn.Close();
                }

                //open reviewGrid by friends
                using (var conn = new NpgsqlConnection(buildConnString()))//bring review data form the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        //cmd.CommandText = "SELECT distinct Users.name, review_date, text FROM  Users, Review, Business, Friends_with WHERE Business.Business_id = Review.Business_id AND Friends_with.user_id = Review.user_id  AND Friends_with.user_id = Users.user_id AND Business.name = '" + selectedBusiness.name.ToString() + "'AND address = '" + selectedBusiness.address.ToString() + "';";
                        if (aUser ==null)
                            cmd.CommandText = "SELECT distinct Users.name, review_date, review_stars, text FROM  Users, Review, Business WHERE Business.Business_id = Review.Business_id AND Users.user_id = Review.user_id  AND Business.name = '" + selectedBusiness.name.ToString() + "'AND address = '" + selectedBusiness.address.ToString() + "';";
                        else
                            cmd.CommandText = "SELECT afriend.name, review_date, review_stars, text FROM  Users as auser, Users as afriend, Review, Business, friends_with " +
                                "WHERE Business.Business_id = Review.Business_id AND afriend.user_id = Review.user_id AND afriend.user_id = friends_with.friends_id AND auser.user_id = friends_with.user_id " +
                                "AND auser.user_id = '" + aUser.user_id + "' AND Business.name = '" + selectedBusiness.name.ToString() + "'AND address = '" + selectedBusiness.address.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reviewGrid.Items.Add(new Review() { user_name = reader.GetString(0), review_date = reader.GetDate(1), review_stars = reader.GetString(2),  review_text = reader.GetString(3) });
                            }
                        }
                    }
                    conn.Close();
                }

                //businessOpenInfoTxt
                //open BusinessOpenInfo
                using (var conn = new NpgsqlConnection(buildConnString()))//bring review data form the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT hours_open, hours_close FROM Business, hours WHERE Business.business_id = hours.business_id " +
                            "AND hours_day = '" + curDay + "' AND  name = '" + selectedBusiness.name.ToString() + "' AND address = '" + selectedBusiness.address.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                businessOpenInfoTxt.Text = "<"+ curDay +">\n";
                                businessOpenInfoTxt.Text += "open > ";
                                businessOpenInfoTxt.Text += reader[0].ToString();
                                businessOpenInfoTxt.Text += "\nclose > ";
                                businessOpenInfoTxt.Text += reader[1].ToString();
                            }
                        }
                        if(businessOpenInfoTxt.Text == "")
                        {
                            businessOpenInfoTxt.Text = "<" + curDay + ">\n Closed";
                        }
                    }
                    conn.Close();
                }
            }
        }

        private void AddRating_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addRating.SelectedIndex > -1)
            {
                if (addRating.SelectedItem.ToString() == "1")
                    selectedRating = 1;
                else if (addRating.SelectedItem.ToString() == "2")
                    selectedRating = 2;
                else if (addRating.SelectedItem.ToString() == "3")
                    selectedRating = 3;
                else if (addRating.SelectedItem.ToString() == "4")
                    selectedRating = 4;
                else if (addRating.SelectedItem.ToString() == "5")
                    selectedRating = 5;

            }
        }

        private void addCategoriesBtn_Click(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            reviewGrid.Items.Clear();
            selectedcategorylist.Items.Add(selectedCategoriesnametoAdd);
            size += 1;

            if (size == 1)
            {
                categoriesQuery = null;
                categoriesArray[0] = selectedCategoriesnametoAdd;
            }
            else
            {
                categoriesArray[size-1] = selectedCategoriesnametoAdd;

            }
            // INTERSECT --  all selected Categories
            updateBusinessGrid();


        }
        private void removeCateoriesBtn_Click(object sender, RoutedEventArgs e)
        {
            businessGrid.Items.Clear();
            reviewGrid.Items.Clear();
            //remove a selected cateory
            selectedcategorylist.Items.Remove(selectedCategoriesnametoRemove);
            //categoriesQuery = categoriesArray[0];
            for (index = 0; index < size; index++)
            {
                if(categoriesArray[index] == selectedCategoriesnametoRemove)
                {
                    categoriesArray[index] = "";
                    size -= 1;

                    for (int i = index; i < size; i++)
                    {
                        categoriesArray[i] = categoriesArray[i + 1];
                        categoriesArray[i+1] = "";
                    }
                    break;
                }
            }

            // INTERSECT --  all selected Categorie
            updateBusinessGrid();

        }

        private void Selectedcategorylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedcategorylist.SelectedIndex > -1)
            {
                selectedCategoriesnametoRemove = selectedcategorylist.SelectedItem.ToString();
            }
        }

        private void SearchedNameList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            friendsGrid.Items.Clear();
            friendsReviewGrid.Items.Clear();
            favoriteBusinessGrid.Items.Clear();
            if (searchedNameList.SelectedIndex > -1)
            {
                selectedUserId = searchedNameList.SelectedItem.ToString();
                //SelectedBusiness = (Business)businessGrid.SelectedItems[0].ToString();
                if (selectedUserId != "")
                {
                    using (var conn = new NpgsqlConnection(buildConnString())) //bring all categories form the selected business on the database
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "SELECT user_id, average_stars, fans, name, cool, funny, useful, yelping_since, user_latitude, user_longitude FROM Users WHERE user_id = '" + selectedUserId + "';";

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    aUser = new User() { user_id = reader.GetString(0), average_stars = reader.GetDouble(1), fans = reader.GetInt32(2), name = reader.GetString(3), cool = reader.GetInt32(4), funny = reader.GetInt32(5), useful = reader.GetInt32(6), yelping_since = reader.GetDate(7), user_latitude = reader.GetDouble(8), user_longitude = reader.GetDouble(9) };
                                    selectedUserName.Text = aUser.name;
                                    selectedUserStars.Text = aUser.average_stars.ToString();
                                    selectedUserFans.Text = aUser.fans.ToString();
                                    selectedUserYelpingSince.Text = aUser.yelping_since.ToString();
                                    selectedUserFunny.Text = aUser.funny.ToString();
                                    selectedUserCool.Text = aUser.cool.ToString();
                                    selectedUserUseful.Text = aUser.useful.ToString();
                                    selectedUserLat.Text = aUser.user_latitude.ToString();
                                    selectedUserLong.Text = aUser.user_longitude.ToString();
                                }
                            }
                        }
                        conn.Close();
                    }

                    //open friends list 
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "SELECT afriend.name, afriend.average_stars, afriend.yelping_since FROM Users as auser, Users as afriend, friends_with " +
                                "WHERE  auser.user_id = friends_with.user_id AND afriend.user_id = friends_with.friends_id AND auser.user_id = '" + selectedUserId + "' ORDER BY afriend.user_id;";
                            using (var reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    friendsGrid.Items.Add(new User() { name = reader.GetString(0), average_stars = reader.GetDouble(1), yelping_since = reader.GetDate(2)});
                                }
                            }
                        }
                        conn.Close();
                    }

                    //open favorites list
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "SELECT business.name, stars, city, zipcode, address FROM Business,favorites, Users WHERE Business.business_id = favorites.business_id AND Users.user_id = favorites.user_id AND Users.user_id = '" + selectedUserId + "';";

                            using (var reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    favoriteBusinessGrid.Items.Add(new Business() { name = reader.GetString(0), stars = reader.GetDouble(1), city = reader.GetString(2), zipcode = reader.GetString(3), address = reader.GetString(4) });
                                }
                            }
                        }
                        conn.Close();
                    }


                    //open Friends' recent review list
                    using (var conn = new NpgsqlConnection(buildConnString()))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = "SELECT users.name, business.name, city, text FROM Business, Users, review " +
                                "WHERE business.business_id = review.business_id AND review.user_id = users.user_id AND (users.user_id, review_date) IN " +
                                "(SELECT afriend.user_id, max(review_date) FROM Business, Users as auser, Users as afriend, friends_with, review " +
                                "WHERE auser.user_id = friends_with.user_id AND afriend.user_id = friends_with.friends_id AND afriend.user_id = review.user_id AND review.business_id = business.business_id  " +
                                "AND auser.user_id = '"+ selectedUserId +"' GROUP BY afriend.user_id) ORDER BY users.user_id;";

                            using (var reader = cmd.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    friendsReviewGrid.Items.Add( new FriendRecentReview { user_name = reader.GetString(0), business_name = reader.GetString(1), city = reader.GetString(2), review_text = reader.GetString(3) });
                                }
                            }
                        }
                        conn.Close();
                    }

                }
            }
        }
   
        private void SortByBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            if (sortByBox.SelectedIndex > -1)
            {
                if (sortByBox.SelectedItem.ToString() == "Business Name")
                    sortOption = "ORDER BY name";
                else if (sortByBox.SelectedItem.ToString() == "Highest Ranking")
                    sortOption = "ORDER BY stars DESC";
                else if (sortByBox.SelectedItem.ToString() == "Most Reviewed")
                    sortOption = "ORDER BY review_count DESC";
                else if (sortByBox.SelectedItem.ToString() == "Best Review Ranking")
                    sortOption = "ORDER BY review_ratings DESC";
                else if (sortByBox.SelectedItem.ToString() == "Most CheckIn")
                    sortOption = "ORDER BY num_checkins DESC";
                else if (sortByBox.SelectedItem.ToString() == "Nearest")
                    sortOption = "ORDER BY distance";
                else
                    sortOption = "ORDER BY name";

            }
            updateBusinessGrid();

        }

        private void updateUserInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            aUser.user_latitude = Convert.ToDouble(selectedUserLat.Text, CultureInfo.InvariantCulture);
            aUser.user_longitude = Convert.ToDouble(selectedUserLong.Text, CultureInfo.InvariantCulture);
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "UPDATE Users SET user_latitude = "+ selectedUserLat.Text +", user_longitude = "+ selectedUserLong.Text +" WHERE user_id = '" + selectedUserId+"'";
                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            MessageBox.Show("User longitude, latitude were saved");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                conn.Close();
            }
        }


        private void favoriteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aUser != null && selectedBusiness !=null )
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Favorites (Business_id , User_id) VALUES  ('" + selectedBusiness.business_id + "','" + aUser.user_id + "');";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                MessageBox.Show("Successfully added into Favorite!");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("User was not selected, or Business was not selected. ");

            }
        }
        private void favoriteBusinessGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedFavorites = (Business)favoriteBusinessGrid.SelectedItem;
            if (selectedFavorites != null)
            {
                using (var conn = new NpgsqlConnection(buildConnString())) //bring all categories form the selected business on the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct business_id FROM Business WHERE name = '" + selectedFavorites.name.ToString() + "' AND address = '" + selectedFavorites.address.ToString() + "';";                      
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectedFavorites.business_id = reader[0].ToString();
                            }
                        }
                    }
                    conn.Close();
                }
            }
        }
        private void removeFromFavoritesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aUser != null && selectedFavorites!=null )
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "DELETE FROM Favorites WHERE business_id = '"+selectedFavorites.business_id+"' AND user_id = '"+ aUser.user_id +"';";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                MessageBox.Show("Successfully deleted from Favorite!");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    conn.Close();

                }
            }
            else
            {
                MessageBox.Show("User was not selected, or Business was not selected. ");
            }
        }
        private void CheckinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aUser != null && selectedBusiness != null)
            {
                using (var conn = new NpgsqlConnection(buildConnString()))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Checkin (business_id, checkin_day, checkin_time, checkin_count) " +
                            "VALUES  ('" + selectedBusiness.business_id + "','" + curDay + "','"+ curTime+"',1);";
                        try
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                MessageBox.Show("Successfully CheckIn! ");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("User was not selected, or Business was not selected. ");
            }
        }

        private void showReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aUser != null && selectedBusiness != null)
            {
                ReviewsGrid newWindowReview = new ReviewsGrid();
                addColumns2ReviewGrid(newWindowReview);
                using (var conn = new NpgsqlConnection(buildConnString()))//bring review data form the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct review_date, Users.name, review_stars, text, Users.funny, Users.useful, USers.cool FROM  Users, Review, Business WHERE Business.Business_id = Review.Business_id AND Users.user_id = Review.user_id  AND Business.name = '" + selectedBusiness.name.ToString() + "' AND address = '" + selectedBusiness.address.ToString() + "';";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                newWindowReview.reviewDataGrid.Items.Add(new Review { review_date = reader.GetDate(0), user_name = reader.GetString(1), review_stars = reader.GetString(2), review_text = reader.GetString(3), funny = reader.GetString(4), useful = reader.GetString(5), cool = reader.GetString(6) });
                            }
                        }
                    }
                    conn.Close();
                }
                newWindowReview.ShowDialog();
            }
            else
            {
                MessageBox.Show("User was not selected, or Business was not selected. ");
            }
        }

        private void showCheckInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (aUser != null && selectedBusiness != null)
            {
                List<KeyValuePair<string, int>> myChartData = new List<KeyValuePair<string, int>>();
                using (var conn = new NpgsqlConnection(buildConnString())) //bring all categories form the selected business on the database
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT distinct checkin_day, SUM(checkin_count) FROM Business, checkin " +
                            "WHERE business.business_id= checkin.business_id AND business.business_id = '" + selectedBusiness.business_id + "' GROUP BY checkin_day;";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                myChartData.Add(new KeyValuePair<string, int>(reader.GetString(0), reader.GetInt32(1)));
                            }
                        }
                    }
                    conn.Close();
                }
                checkInChart aChart = new checkInChart();
                aChart.CheckInChart.DataContext = myChartData;
                aChart.ShowDialog();
            }
            else
            {
                MessageBox.Show("User was not selected, or Business was not selected. ");
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            updateBusinessGrid();
        }

        private void SearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchedNameList.Items.Clear();
            friendsGrid.Items.Clear();
            friendsReviewGrid.Items.Clear();

            //searchedNameList.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT user_id FROM Users WHERE name = '" + searchName.Text + "' ORDER BY user_id;";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            searchedNameList.Items.Add(reader.GetString(0) );
                        }
                    }
                }
                conn.Close();
            }
        }

        private void RefreshFavoriteGridBtn_Click(object sender, RoutedEventArgs e)
        {
            favoriteBusinessGrid.Items.Clear();
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT business.name, stars, city, zipcode, address FROM Business,favorites, Users WHERE Business.business_id = favorites.business_id AND Users.user_id = favorites.user_id AND Users.user_id = '" + selectedUserId + "';";

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            favoriteBusinessGrid.Items.Add(new Business() { name = reader.GetString(0), stars = reader.GetDouble(1), city = reader.GetString(2), zipcode = reader.GetString(3), address = reader.GetString(4) });
                        }
                    }
                }
                conn.Close();
            }
        }

        public void updateBusinessGrid()
        {
            reviewGrid.Items.Clear();
            businessGrid.Items.Clear();
            businessName.Clear();
            businessAddress.Clear();
            categoriesQuery = "SELECT distinct name, state, city, address,  " +
              "(2 * 3961 * asin(sqrt((sin(radians((latitude - " + aUser.user_latitude + ") / 2))) ^ 2 + cos(radians(" + aUser.user_latitude + ")) * cos(radians(latitude)) * (sin(radians((longitude - " + aUser.user_longitude + ") / 2))) ^ 2))) AS distance, " +
              " stars, review_count, review_ratings, num_checkins FROM business, Categories WHERE Business.Business_id = Categories.Business_id " +
              "AND zipcode = '" + zipcodelist.SelectedItem.ToString() + "' AND city = '" + citylist.SelectedItem.ToString() + "' AND state = '" + statelist.SelectedItem.ToString() + "' ";

            for (index = 0; index < size; index++)
            {
                categoriesQuery = '(' + categoriesQuery + ") INTERSECT ";
                categoriesQuery += "(SELECT distinct name, state, city, address, " +
                    "(2 * 3961 * asin(sqrt((sin(radians((latitude - " + aUser.user_latitude + ") / 2))) ^ 2 + cos(radians(" + aUser.user_latitude + ")) * cos(radians(latitude)) * (sin(radians((longitude - " + aUser.user_longitude + ") / 2))) ^ 2))) AS distance, " +
                    " stars, review_count, review_ratings, num_checkins FROM business, Categories WHERE Business.Business_id = Categories.Business_id AND categories_name = '" + categoriesArray[index] + "' " +
                    "AND zipcode = '" + zipcodelist.SelectedItem.ToString() + "' AND city = '" + citylist.SelectedItem.ToString() + "' AND state = '" + statelist.SelectedItem.ToString() + "' )";
            }
            categoriesQuery += sortOption;
            using (var conn = new NpgsqlConnection(buildConnString()))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = categoriesQuery;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            businessGrid.Items.Add(new Business() { name = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2), address = reader.GetString(3), distance = reader.GetDouble(4), stars = reader.GetDouble(5), review_count = reader.GetInt32(6), review_ratings = reader.GetDouble(7), num_checkins = reader.GetInt32(8) });
                        }
                    }
                }
                conn.Close();
            }
        }
    }
}