/**
 *
 *   Copyright (c) 2014 Entropa Software Ltd.  All Rights Reserved.    
 *
 */
using System;
using System.IO;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtils;

namespace TestTestUtils {

	/// <summary>
	/// This tests the FileAssert methods.
	/// </summary>
	[TestClass]
	public class FileAssertTest {
		
		[TestInitialize]
		public void Setup() {
			log4net.Config.XmlConfigurator.Configure( new FileInfo( "log4net.config" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_1vs1() {
			FileAssert.AreEqual( @"Resources\FileAssertTest\test1.txt", @"Resources\FileAssertTest\test1.txt" );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_1vs2() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test1.txt", 
				@"Resources\FileAssertTest\test2.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test1.txt (871 bytes) with test2.txt (875 bytes) - Files differ at line 3, char 55 - File 1: ' from the *far past. S', File 2: ' from the *distant pas'",
				@"FileAssert.AreEqual failed. Comparing expected (871 bytes) with actual (875 bytes) - Files differ at line 3, char 55 - File 1: ' from the *far past. S', File 2: ' from the *distant pas'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_1vs3() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test1.txt", 
				@"Resources\FileAssertTest\test3.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test1.txt (871 bytes) with test3.txt (846 bytes) - Files differ at line 6, char 33 - File 1: ' explained*: “I have n', File 2: ' explained*\EOL'",
				@"FileAssert.AreEqual failed. Comparing expected (871 bytes) with actual (846 bytes) - Files differ at line 6, char 33 - File 1: ' explained*: “I have n', File 2: ' explained*\EOL'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_1vs4() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test1.txt", 
				@"Resources\FileAssertTest\test4.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test1.txt (871 bytes) with test4.txt (810 bytes) - Files differ at line 6- File 1: ' a life se...', File 2: EOF",
				@"FileAssert.AreEqual failed. Comparing expected (871 bytes) with actual (810 bytes) - Files differ at line 6- File 1: ' a life se...', File 2: EOF"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_1vs5() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test1.txt", 
				@"Resources\FileAssertTest\test5.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test1.txt (871 bytes) with test5.txt (870 bytes) - Files differ at line 5, char 160 - File 1: 'onal basis*,', File 2: 'onal basis*\EOL'",
				@"FileAssert.AreEqual failed. Comparing expected (871 bytes) with actual (870 bytes) - Files differ at line 5, char 160 - File 1: 'onal basis*,', File 2: 'onal basis*\EOL'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_2vs1() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test2.txt", 
				@"Resources\FileAssertTest\test1.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test2.txt (875 bytes) with test1.txt (871 bytes) - Files differ at line 3, char 55 - File 1: ' from the *distant pas', File 2: ' from the *far past. S'",
				@"FileAssert.AreEqual failed. Comparing expected (875 bytes) with actual (871 bytes) - Files differ at line 3, char 55 - File 1: ' from the *distant pas', File 2: ' from the *far past. S'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_3vs1() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test3.txt", 
				@"Resources\FileAssertTest\test1.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test3.txt (846 bytes) with test1.txt (871 bytes) - Files differ at line 6, char 33 - File 1: ' explained*\EOL', File 2: ' explained*: “I have n'",
				@"FileAssert.AreEqual failed. Comparing expected (846 bytes) with actual (871 bytes) - Files differ at line 6, char 33 - File 1: ' explained*\EOL', File 2: ' explained*: “I have n'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_4vs1() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test4.txt", 
				@"Resources\FileAssertTest\test1.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test4.txt (810 bytes) with test1.txt (871 bytes) - Files differ at line 6- File 1: EOF, File 2: ' a life se...'",
				@"FileAssert.AreEqual failed. Comparing expected (810 bytes) with actual (871 bytes) - Files differ at line 6- File 1: EOF, File 2: ' a life se...'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_5vs1() {
			TestFileAssertException(
				@"Resources\FileAssertTest\test5.txt", 
				@"Resources\FileAssertTest\test1.txt",
				null,
				@"FileAssert.AreEqual failed. Comparing test5.txt (870 bytes) with test1.txt (871 bytes) - Files differ at line 5, char 160 - File 1: 'onal basis*\EOL', File 2: 'onal basis*,'",
				@"FileAssert.AreEqual failed. Comparing expected (870 bytes) with actual (871 bytes) - Files differ at line 5, char 160 - File 1: 'onal basis*\EOL', File 2: 'onal basis*,'"
				);
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_FileInfo_1vs1() {
			FileAssert.AreEqual( new FileInfo( @"Resources\FileAssertTest\test1.txt" ), new FileInfo( @"Resources\FileAssertTest\test1.txt" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "actual" )]
		public void TestAreEqual_FileInfo_NullActual() {
			FileAssert.AreEqual( new FileInfo( @"Resources\FileAssertTest\test1.txt" ), null );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "expected" )]
		public void TestAreEqual_FileInfo_NullExpected() {
			FileAssert.AreEqual( null, new FileInfo( @"Resources\FileAssertTest\test1.txt" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		public void TestAreEqual_Stream_1vs1() {
			using ( FileStream expectedStream = new FileStream( @"Resources\FileAssertTest\test1.txt", FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				using ( FileStream actualStream = new FileStream( @"Resources\FileAssertTest\test1.txt", FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
					FileAssert.AreEqual( expectedStream, actualStream );
				}
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "actual" )]
		public void TestAreEqual_Stream_NullActual() {
			using ( FileStream stream = new FileStream( @"Resources\FileAssertTest\test1.txt", FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				FileAssert.AreEqual( stream, null );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "expected" )]
		public void TestAreEqual_Stream_NullExpected() {
			using ( FileStream stream = new FileStream( @"Resources\FileAssertTest\test1.txt", FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
				FileAssert.AreEqual( null, stream );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "actual" )]
		public void TestAreEqual_String_NullActual() {
			FileAssert.AreEqual( @"Resources\FileAssertTest\test1.txt", null );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\FileAssertTest", @"Resources\FileAssertTest" )]
		[ExpectedException( typeof( ArgumentNullException ), "expected" )]
		public void TestAreEqual_String_NullExpected() {
			FileAssert.AreEqual( null, @"Resources\FileAssertTest\test1.txt" );
		}

		/// <summary>
		/// Tests file assertion.
		/// </summary>
		/// <param name="expected"></param>
		/// <param name="actual"></param>
		/// <param name="message"></param>
		/// <param name="fileMessage"></param>
		/// <param name="streamMessage"></param>
		private static void TestFileAssertException( string expected, string actual, string message, string fileMessage, string streamMessage ) {
			// Test string
			try {
				FileAssert.AreEqual( expected, actual, message );
				Assert.Fail( "Expected AssertFailedException" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( fileMessage, afe.Message );
			}
			// Test FileInfo
			try {
				FileAssert.AreEqual( new FileInfo( expected ), new FileInfo( actual ), message );
				Assert.Fail( "Expected AssertFailedException" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( fileMessage, afe.Message );
			}
			// Test Stream
			try {
				using ( FileStream expectedStream = new FileStream( expected, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
					using ( FileStream actualStream = new FileStream( actual, FileMode.Open, FileAccess.Read, FileShare.Read ) ) {
						FileAssert.AreEqual( expectedStream, actualStream, message );
					}
				}
				Assert.Fail( "Expected AssertFailedException" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( streamMessage, afe.Message );
			}
		}

	}
}
