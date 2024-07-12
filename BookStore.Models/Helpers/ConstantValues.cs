using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Helpers
{
    public static class ConstantValues
    {
        //Response Messages

        #region User

        public static string SuccessMSGAddUser = "User Added Successfully.";
        public static string SuccessMSGUpdateUser = "User Updated Successfully.";
        public static string SuccessMSGDeleteUser = "User Deleted Successfully.";
        public static string ErrorMSGAddUser = "User Already Exsists!s";

        public static string NotFoundMSGUser = "User Not Found!";

        public static string CheckCredentialsMSGUser = "Please check creadentials and try again!";
        public static string SuccessMSGLoginUser = "User Logged in Successfully.";


        #endregion

        #region Category
        public static string SuccessMSGAddCategory = "Category Added Successfully.";
        public static string SuccessMSGUpdatedCategory = "Category Updated Successfully.";
        public static string SuccessMSGDeleteCategory = "Category Deleted Successfully.";

        public static string NotFoundMSGCategory = "Category Not Found!";


        #endregion

        #region Book

        public static string SuccessMSGAddBook = "Book Added Successfully.";
        public static string SuccessMSGUpdatedBook = "Book Updated Successfully.";
        public static string SuccessMSGDeleteBook = "Book Deleted Successfully.";
        public static string NotFoundMSGBook = "Book Not Found!";

        #endregion

        #region Purchase
        public static string SuccessMSGAddPurchase = "Purchase Added Successfully.";
        public static string SuccessMSGUpdatedPurchase = "Purchase Updated Successfully.";
        public static string SuccessMSGDeletePurchase = "Purchase Deleted Successfully.";
        public static string NotFoundMSGPurchase = "Purchase Not Found!";
        #endregion
    }
}
