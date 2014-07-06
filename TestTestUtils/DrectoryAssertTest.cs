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
	public class DirectoryAssertTest {
		
		[TestInitialize]
		public void Setup() {
			log4net.Config.XmlConfigurator.Configure( new FileInfo( "log4net.config" ) );
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir1" );
			string input2 = Path.Combine( baseDir, "dir2" );
			DirectoryAssert.AreEqual( input1, input2 );
			// Should pass
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual_ChangedText() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir1" );
			string input2 = Path.Combine( baseDir, "dir4" );
			try {
				DirectoryAssert.AreEqual( input1, input2 );
				Assert.Fail( "Expected assertion exception" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( @"DirectoryAssert.AreEqual failed. Comparing test1.txt (871 bytes) with test1.txt (872 bytes) - Files differ at line 3, char 1 - File 1: '*Sometimes, ', File 2: '*XXXmetimes,'", afe.Message );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual_ExtraDir() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir1" );
			string input2 = Path.Combine( baseDir, "dir6" );
			try {
				DirectoryAssert.AreEqual( input1, input2 );
				Assert.Fail( "Expected assertion exception" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( @"DirectoryAssert.AreEqual failed. Comparing dir1\a with dir6\a - dir6\a contains additional subdirectory 'b'", afe.Message );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual_ExtraFile() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir1" );
			string input2 = Path.Combine( baseDir, "dir3" );
			try {
				DirectoryAssert.AreEqual( input1, input2 );
				Assert.Fail( "Expected assertion exception" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( @"DirectoryAssert.AreEqual failed. Comparing dir1 with dir3 - dir3 contains additional file 'extrafile.txt'", afe.Message );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual_MissingDir() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir6" );
			string input2 = Path.Combine( baseDir, "dir1" );
			try {
				DirectoryAssert.AreEqual( input1, input2 );
				Assert.Fail( "Expected assertion exception" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( @"DirectoryAssert.AreEqual failed. Comparing dir6\a with dir1\a - dir1\a is missing subdirectory 'b'", afe.Message );
			}
		}

		[TestMethod]
		[DeploymentItem( "log4net.config" )]
		[DeploymentItem( @"Resources\DirectoryAssertTest", @"Resources\DirectoryAssertTest" )]
		public void TestAreEqual_MissingFile() {
			const string baseDir = @"Resources\DirectoryAssertTest";
			string input1 = Path.Combine( baseDir, "dir3" );
			string input2 = Path.Combine( baseDir, "dir1" );
			try {
				DirectoryAssert.AreEqual( input1, input2 );
				Assert.Fail( "Expected assertion exception" );
			} catch ( AssertFailedException afe ) {
				Assert.AreEqual( @"DirectoryAssert.AreEqual failed. Comparing dir3 with dir1 - dir1 is missing file 'extrafile.txt'", afe.Message );
			}
		}
	}
}
