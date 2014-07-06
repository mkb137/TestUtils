/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUtils {

	/// <summary>
	/// Provides file-related assertions.
	/// </summary>
	public static class FileAssert {

		/// <summary>
		/// Asserts that the files at the given location are identical.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		public static void AreEqual( string expected, string actual, string message = null ) {
			if ( String.IsNullOrEmpty( expected ) ) throw new ArgumentNullException( "expected" );
			if ( String.IsNullOrEmpty( actual   ) ) throw new ArgumentNullException( "actual"   );
			AreEqual( new FileInfo( expected ), new FileInfo( actual ), message );
		}

		/// <summary>
		/// Asserts that the files at the given location are identical.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		public static void AreEqual( FileInfo expected, FileInfo actual, string message = null ) {
			if ( null == expected ) throw new ArgumentNullException( "expected" );
			if ( null == actual   ) throw new ArgumentNullException( "actual"   );
			if ( !expected.Exists ) throw new FileNotFoundException( String.Format( Strings.FileAssert_ExpectedFileNotFound, expected.FullName, message ), expected.FullName );
			if ( !actual.Exists   ) throw new FileNotFoundException( String.Format( Strings.FileAssert_ActualFileNotFound,   actual.FullName,   message ), actual.FullName   );
			using ( FileStream expectedStream = new FileStream( expected.FullName, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				using ( FileStream actualStream = new FileStream( actual.FullName, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
					AreEqual( expectedStream, actualStream, message, "FileAssert.AreEqual", expected.Name, actual.Name );
				}
			}
		}

		/// <summary>
		/// Asserts that the files at the given location are identical.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		public static void AreEqual( Stream expected, Stream actual, string message = null ) {
			if ( null == expected ) throw new ArgumentNullException( "expected" );
			if ( null == actual   ) throw new ArgumentNullException( "actual"   );
			AreEqual( expected, actual, message, "FileAssert.AreEqual" );
		}

		/// <summary>
		/// Asserts that two files are equal.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		/// <param name="exceptionName"></param>
		/// <param name="expectedFileName"></param>
		/// <param name="actualFileName"></param>
		internal static void AreEqual( FileInfo expected, FileInfo actual, string message, string exceptionName, string expectedFileName = null, string actualFileName = null ) {
			using ( FileStream expectedStream = new FileStream( expected.FullName, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				using ( FileStream actualStream = new FileStream( actual.FullName, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
					AreEqual( expectedStream, actualStream, message, exceptionName, expected.Name, actual.Name );
				}
			}
		}

		/// <summary>
		/// Asserts that two files are equal.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		/// <param name="exceptionName"></param>
		/// <param name="expectedFileName"></param>
		/// <param name="actualFileName"></param>
		internal static void AreEqual( Stream expected, Stream actual, string message, string exceptionName, string expectedFileName = null, string actualFileName = null ) {
			// If we have no file names, just say "expected" and "actual".
			if ( null == expectedFileName ) {
				expectedFileName = Strings.Expected;
			}
			if ( null == actualFileName ) {
				actualFileName = Strings.Actual;
			}
			int lineCount = 0;
			using ( StreamReader reader1 = new StreamReader( expected ) ) {
				using ( StreamReader reader2 = new StreamReader( actual ) ) {
					long expecedLength = expected.Length;
					long actualLength = actual.Length;
					string line1;
					do {
						// Read a line from each file
						lineCount++;
						line1 = reader1.ReadLine();
						string line2 = reader2.ReadLine();
						// If the lines are not equal...
						if ( !String.Equals( line1, line2, StringComparison.Ordinal ) ) {
							StringBuilder sb = new StringBuilder();
							sb.AppendFormat( Strings.FileAssert_LinePosition,
								exceptionName,
								expectedFileName,
								expecedLength,
								actualFileName,
								actualLength,
								lineCount
								);
							// If line1 is null...
							if ( null == line1 ) {
								sb.AppendFormat( Strings.FileAssert_ExpectedEmpty, FilePreview( line2 ) );
							}
							// If line2 is null...
							else if ( null == line2 ) {
								sb.AppendFormat( Strings.FileAssert_ActualEmpty, FilePreview( line1 ) );
							}
							// If the lines are just different...
							else {
								// Go through character by character
								int minLength = Math.Min( line1.Length, line2.Length );
								// Get the index of where they differ
								int index = minLength;
								for ( int i = 0; i < minLength; i++ ) {
									char c1 = line1[i];
									char c2 = line2[i];
									if ( c1 != c2 ) {
										index = i;
										break;
									}
								}
								sb.AppendFormat( Strings.FileAssert_CharPosition, index + 1 );
								// Try to form a message that gets a little before and a little after the difference
								sb.AppendFormat(
									Strings.FileAssert_FilesDiffer, 
									GetSampleSubstring( line1, index ),
									GetSampleSubstring( line2, index ) 
									);
							}
							if ( !String.IsNullOrEmpty( message ) ) {
								sb.Append( " - " );
								sb.Append( message );
							}
							throw new AssertFailedException( sb.ToString() );
						}
					} while ( null != line1 );
				}
			}
		}

		private static string FilePreview( string line2 ) {
			string x;
			if ( line2.Length > 10 ) {
				x = String.Format( "'{0}...'", line2.Substring( 0, 10 ) );
			} else {
				x = String.Format( "'{0}'", line2 );
			}
			return x;
		}

		/// <summary>
		/// Pulls a sample substring from a line.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		private static string GetSampleSubstring( string line, int index ) {
			StringBuilder sb = new StringBuilder();
			// Set the length of the string we'll try to pull off before and after the index
			const int sampleLength = 10;

			int startIndex = Math.Max( 0, index - sampleLength );
			int startLength = Math.Min( startIndex, sampleLength );
			sb.Append( line.Substring( startIndex, startLength ) );

			sb.Append( Strings.FileAssert_DifferenceIndicator );
			if ( index < line.Length ) {
				sb.Append( line.Substring( index, 1 ) );
				int endIndex = index + 1;
				if ( line.Length > endIndex ) {
					int endLength = Math.Min( line.Length - endIndex, sampleLength );
					sb.Append( line.Substring( endIndex, endLength ) );
				}
			} else {
				sb.Append( Strings.FileAssert_EOL );
			}
			return sb.ToString();
		}
	}
}
