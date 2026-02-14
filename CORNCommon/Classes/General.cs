using System;
using System.Collections;



namespace CORNCommon.Classes
{
	public delegate void MessageMaker(string message);
	/// <summary>
	/// General
	/// <author>Rizwan Ansari</author>
	/// <date>22-08-2007</date>
	/// </summary>
	public class General
	{	
		#region Constructors
		public General()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion
				
		#region Private Variables
		#endregion

		#region public Properties
		#endregion
		
		#region public static Methods
		public static string ReplaceSymbols(string p_FormattedString)
		{
			try
			{
				p_FormattedString = p_FormattedString.Replace("%APPPATH%", Configuration.GetAppInstallationPath());
				p_FormattedString = p_FormattedString.Replace("%SERVERFTPPATH%", Configuration.ServerFTPpath);
			}
			catch(Exception exp)
			{
				throw exp;
			}

			return p_FormattedString;
		}
		
		// added by Ali Raza
		public static void ShowErrorMessage(string errMsg)
		{
			System.Windows.Forms.MessageBox.Show(errMsg,"CORN",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning );
		}
        
		#endregion

		#region Private Methods
		#endregion
		
		#region public Methods
		#endregion
	}
}
