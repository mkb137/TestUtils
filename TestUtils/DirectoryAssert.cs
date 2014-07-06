/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtils {

	/// <summary>
	/// This contains methods for testing that two directories contain the same contents.
	/// </summary>
	public static class DirectoryAssert {

		/// <summary>
		/// Tests that the number and contents of all files in the directories are the same.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		public static void AreEqual( string expected, string actual, string message = null ) {
			if ( String.IsNullOrEmpty( expected ) ) throw new ArgumentNullException( "expected" );
			if ( String.IsNullOrEmpty( actual   ) ) throw new ArgumentNullException( "actual"   );
			AreEqual( new DirectoryInfo( expected ), new DirectoryInfo( actual ), message );
		}

		/// <summary>
		/// Tests that the number and contents of all files in the directories are the same.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		public static void AreEqual( DirectoryInfo expected, DirectoryInfo actual, string message = null ) {
			if ( null == expected ) throw new ArgumentNullException( "expected" );
			if ( null == actual   ) throw new ArgumentNullException( "actual"   );
			AreEqual( expected, actual, String.Empty, String.Empty, message );
		}
		/// <summary>
		/// Asserts that the contents of two directories, including number, name, and content of all files and subdirectories, is the same.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="path1">The parent path of dir1.</param>
		/// <param name="path2">The parent path of dir2.</param>
		/// <param name="message"></param>
		private static void AreEqual( DirectoryInfo expected, DirectoryInfo actual, string path1, string path2, string message ) {
			// Ensure that both directories exist
			Assert.IsTrue( expected.Exists, String.Format( Strings.DirectoryAssert_ExpectedDirectoryNotFound, expected.FullName, message ) );
			Assert.IsTrue( actual.Exists,   String.Format( Strings.DirectoryAssert_ActualDirectoryNotFound,   actual.FullName,   message ) );
			// Get the names of the files in each directory
			List<String> fileNames1 = GetFileNames( expected );
			List<String> fileNames2 = GetFileNames( actual );
			// For each file in 1...
			foreach ( string fileName1 in fileNames1 ) {
				// If the file exists in both...
				if ( fileNames2.Contains( fileName1 ) ) {
					// Check that the contents of both files is the same
					FileAssert.AreEqual(
						new FileInfo( Path.Combine( expected.FullName, fileName1 ) ),
						new FileInfo( Path.Combine( actual.FullName, fileName1 ) ), 
						message,
						"DirectoryAssert.AreEqual"
						);
				} else {
					// The file is missing from dir2.
					throw new AssertFailedException( String.Format( Strings.DirectoryAssert_MissingFile,
						Path.Combine( path1, expected.Name ),
						Path.Combine( path2, actual.Name ),
						fileName1
						));
				}
			}
			// For each file in 2...
			foreach ( string fileName2 in fileNames2 ) {
				// If the file doesn't exist in 1...
				if ( !fileNames1.Contains( fileName2 ) ) {
					// The file is extra in dir2.
					throw new AssertFailedException( String.Format( Strings.DirectoryAssert_ExtraFile,
						Path.Combine( path1, expected.Name ),
						Path.Combine( path2, actual.Name ),
						fileName2
						));
				}
			}
			// Get the names of the subdirectories in each directory
			List<String> subDirectoryNames1 = GetSubdirectoryNames( expected );
			List<String> subDirectoryNames2 = GetSubdirectoryNames( actual );
			// For each subdir in 1...
			foreach ( string subDirectoryName in subDirectoryNames1 ) {
				// If the subdir exists in both...
				if ( subDirectoryNames2.Contains( subDirectoryName ) ) {
					// Check that the contents of both subdirs is the same
					AreEqual( 
						new DirectoryInfo( Path.Combine( expected.FullName, subDirectoryName ) ),
						new DirectoryInfo( Path.Combine( actual.FullName, subDirectoryName ) ),
						Path.Combine( path1, expected.Name ),
						Path.Combine( path2, actual.Name ),
						message );
				} else {
					// The directory is missing from dir2.
					throw new AssertFailedException( String.Format( Strings.DirectoryAssert_MissingDirectory,
						Path.Combine( path1, expected.Name ),
						Path.Combine( path2, actual.Name ),
						subDirectoryName
						));
				}
			}
			// For each file in 2...
			foreach ( string subDirectoryName2 in subDirectoryNames2 ) {
				// If the subdir doesn't exist in 1...
				if ( !subDirectoryNames1.Contains( subDirectoryName2 ) ) {
					// The subdir is extra in dir2.
					throw new AssertFailedException( String.Format( Strings.DirectoryAssert_ExtraDirectory,
						Path.Combine( path1, expected.Name ),
						Path.Combine( path2, actual.Name ),
						subDirectoryName2
						));
				}
			}
		}

		/// <summary>
		/// Gets the names of the files in the directory as a list.
		/// </summary>
		/// <param name="directoryInfo"></param>
		/// <returns></returns>
		private static List<string> GetFileNames( DirectoryInfo directoryInfo ) {
			// Create a list of file names
			List<String> fileNames = new List<string>();
			// For each file in the directory...
			foreach ( FileInfo fileInfo in directoryInfo.EnumerateFiles() ) {
				// Add the file's name to our list
				fileNames.Add( fileInfo.Name );
			}
			// Return the list of file names.
			return fileNames;
		}

		/// <summary>
		/// Gets the names of the subdirectories in the directory as a list.
		/// </summary>
		/// <param name="directoryInfo"></param>
		/// <returns></returns>
		private static List<string> GetSubdirectoryNames( DirectoryInfo directoryInfo ) {
			// Create a list of subdirectory names
			List<String> subdirectoryNames = new List<string>();
			// For each subdirectory in the directory...
			foreach ( DirectoryInfo subDirectoryInfo in directoryInfo.EnumerateDirectories() ) {
				// Add the subdirectory's name to our list
				subdirectoryNames.Add( subDirectoryInfo.Name );
			}
			// Return the list of file names.
			return subdirectoryNames;
		}
	}
}
