using SmartHotelBookingSystem.DataAccess.ADO;
using SmartHotelBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SmartHotelBookingSystem.BusinessLogicLayer
{
    public class HotelBLL
    {
        private readonly DB1 _dalObject;

        public HotelBLL(DB1 dalObject)
        {
            _dalObject = dalObject;
        }

        #region HotelTable
        public int InsertHotel(Hotel hotel)
        {
            string insertQuery = @"INSERT INTO [SmartHotelDB].[dbo].[Hotels]
                                            ([Name]
                                            ,[Location]
                                            ,[ManagerID]
                                            ,[Amenities]
                                            ,[Rating]
                                            ,[IsActive])
                                            VALUES                                     
                                            (@Name
                                            ,@Location
                                            ,@ManagerID
                                            ,@Amenities
                                            ,@Rating
                                            ,1)";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@Name", hotel.Name));
            nvp.Add(new nameValuePair("@Location", hotel.Location));
            nvp.Add(new nameValuePair("@ManagerID", hotel.ManagerID));
            nvp.Add(new nameValuePair("@Amenities", hotel.Amenities));
            nvp.Add(new nameValuePair("@Rating", hotel.Rating));

            int insertStatus = 0;

            try
            {
                insertStatus = _dalObject.InsertUpdateOrDelete(insertQuery, nvp, false);
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error: " + exp.Message);
            }

            return insertStatus;
        }

        public int UpdateHotel(Hotel hotel, int id)
        {
            string updateQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotels]
                           SET [Name] = @Name,
                               [Location] = @Location,
                               [ManagerID] = @ManagerID,
                               [Amenities] = @Amenities,
                               [Rating] = @Rating,
                               [IsActive] = @IsActive
                                WHERE [HotelID] = @Id";
            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@Id", id));
            nvp.Add(new nameValuePair("@Name", hotel.Name));
            nvp.Add(new nameValuePair("@Location", hotel.Location));
            nvp.Add(new nameValuePair("@ManagerID", hotel.ManagerID));
            nvp.Add(new nameValuePair("@Amenities", hotel.Amenities));
            nvp.Add(new nameValuePair("@Rating", hotel.Rating));
            nvp.Add(new nameValuePair("@IsActive", hotel.IsActive));

            int updateStatus = _dalObject.InsertUpdateOrDelete(updateQuery, nvp, false);
            return updateStatus;
        }

        public DataTable DeleteHotel(int id)
        {
            string deleteHotelQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotels]
                                SET [IsActive] = 0
                                WHERE [HotelID] = @HotelID";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@HotelID", id));

            int deleteStatus = _dalObject.InsertUpdateOrDelete(deleteHotelQuery, nvp, false);

            if (deleteStatus > 0)
            {
                string fetchHotelsQuery = @"SELECT [Name]
                                        FROM [SmartHotelDB].[dbo].[Hotels]
                                        WHERE [IsActive] = 1";

                DataTable dt = _dalObject.FetchData(fetchHotelsQuery);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable SearchHotel(string name)
        {
            string searchHotelQuery = @"SELECT 
                                 [HotelID],
                                 [Name],
                                 [Location],
                                 [ManagerID],
                                 [Amenities], 
                                 [Rating],
                                 [IsActive]
                                FROM 
                                [SmartHotelDB].[dbo].[Hotels]
                                WHERE 
                                [Name] LIKE '%' + @Name + '%'";
            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@Name", name));

            DataTable dt = _dalObject.FillAndReturnDataTable(searchHotelQuery, nvp);
            return dt;
        }

        public DataTable SearchHotel()
        {
            string searchHotelQuery = @"SELECT *
                                FROM 
                                [SmartHotelDB].[dbo].[Hotels]";
            DataTable dt = _dalObject.FetchData(searchHotelQuery);
            return dt;
        }

        public DataTable UpdateAmenities(Hotel hotel)
        {
            string updateAmenitiesQuery = @"UPDATE [SmartHotelDB].[dbo].[Hotels]
                                    SET [Amenities] = @Amenities
                                    WHERE [HotelID] = @HotelID and IsActive = 1";

            nameValuePairList nvp = new nameValuePairList();
            nvp.Add(new nameValuePair("@Amenities", hotel.Amenities));
            nvp.Add(new nameValuePair("@HotelID", hotel.HotelID));

            DataTable dt = _dalObject.FillAndReturnDataTable(updateAmenitiesQuery, nvp);
            return dt;
        }

        public List<Hotel> ConvertDataTableToList(DataTable dataTable)
        {
            var hotelList = new List<Hotel>();

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var hotel = new Hotel
                    {
                        HotelID = row.Field<int?>("HotelID") ?? 0,
                        Name = row.Field<string>("Name") ?? string.Empty,
                        Location = row.Field<string>("Location") ?? string.Empty,
                        ManagerID = row.Field<int?>("ManagerID") ?? 0,
                        Amenities = row.Field<string>("Amenities") ?? string.Empty,
                        Rating = row.Field<double?>("Rating") ?? 0,
                        IsActive = row.Field<bool?>("IsActive") ?? false
                    };

                    hotelList.Add(hotel);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error converting DataRow to Hotel: {ex.Message}", ex);
                }
            }

            return hotelList;
        }

        public List<Hotel> GetAllHotels()
        {
            string fetchHotelsQuery = "select * from [SmartHotelDB].[dbo].[Hotels] where IsActive = 1";

            DataTable dt = _dalObject.FetchData(fetchHotelsQuery);
            List<Hotel> hotelList = ConvertDataTableToList(dt);
            return hotelList;
        }
        #endregion
    }
}
