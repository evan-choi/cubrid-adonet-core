/*
 * Copyright (C) 2008 Search Solution Corporation. All rights reserved by Search Solution. 
 *
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met: 
 *
 * - Redistributions of source code must retain the above copyright notice, 
 *   this list of conditions and the following disclaimer. 
 *
 * - Redistributions in binary form must reproduce the above copyright notice, 
 *   this list of conditions and the following disclaimer in the documentation 
 *   and/or other materials provided with the distribution. 
 *
 * - Neither the name of the <ORGANIZATION> nor the names of its contributors 
 *   may be used to endorse or promote products derived from this software without 
 *   specific prior written permission. 
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
 * OF SUCH DAMAGE. 
 *
 */

/*
 * Modified by Kai.Yang 2013.01.06
 * Add a property autocommit, used to turn ON/OFF of auto-commit mode.
 * Modified constructor fuction, add a parameter bool autoCommit.
 * Modified Initialize function, parse autocommit parameter.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace CUBRID.Data
{
  /// <summary>
  ///   CUBRID implementation of <see cref="T:System.Data.Common.DbConnectionStringBuilder" /> class.
  /// </summary>
  [DefaultProperty("DataSource")]
  public sealed class CUBRIDConnectionStringBuilder : DbConnectionStringBuilder
  {
    /// <summary>
    ///   Valid properties names in the connection string
    /// </summary>
    private static readonly List<string> validKeywords = new List<string>
                                                           {
                                                             "user",
                                                             "password",
                                                             "database",
                                                             "server",
                                                             "port",
                                                             "charset",
                                                             "autocommit"
                                                           };

    /// <summary>
    ///   Connection properties values
    /// </summary>
    private readonly Dictionary<string, string> connProperties =
      new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///   Initializes a new instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    public CUBRIDConnectionStringBuilder()
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    /// <param name="server"> The server address. </param>
    /// <param name="port"> The broker port. </param>
    /// <param name="database"> The CUBRID database. </param>
    /// <param name="user"> The user id. </param>
    /// <param name="password"> The password. </param>
    /// <param name="charset"> Character set of database to be connected. </param>
    /// <param name="autoCommit"> Turn ON/OFF of auto-commit mode. </param>
    public CUBRIDConnectionStringBuilder(string server, string port, string database, string user,
                                         string password, string charset, bool autoCommit)
      : this()
    {
      Server = server;
      Port = port;
      Database = database;
      User = user;
      Password = password;
      Encoding = charset;
      AutoCommit = autoCommit ? "1" : "0";

      string str = BuildConnStringFromProperties();
      ConnectionString = str;
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    /// <param name="server"> The server. </param>
    /// <param name="port"> The port. </param>
    /// <param name="database"> The database. </param>
    /// <param name="user"> The user name. </param>
    /// <param name="password"> The password. </param>
    /// <param name="encoding"> Character set of database to be connected. </param>
    /// <param name="autoCommit"> Turn ON/OFF of auto-commit mode. </param>
    public CUBRIDConnectionStringBuilder(string server, int port, string database, string user,
                                         string password, string encoding, bool autoCommit)
      : this(server, port.ToString(), database, user, password, encoding, autoCommit)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    /// <param name="server"> The server. </param>
    /// <param name="database"> The database. </param>
    /// <param name="user"> The user name. </param>
    /// <param name="password"> The password. </param>
    /// <param name="encoding"> Character set of database to be connected. </param>
    /// <param name="autoCommit"> Turn ON/OFF of auto-commit mode. </param>
    public CUBRIDConnectionStringBuilder(string server, string database, string user, string password,
                                         string encoding, bool autoCommit)
      : this(server, "33000", database, user, password, encoding, autoCommit)
    {
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    /// <param name="connString"> The connection string. </param>
    public CUBRIDConnectionStringBuilder(string connString)
      : this()
    {
      string formattedConnString = Initialize(connString);

      ConnectionString = !String.IsNullOrEmpty(formattedConnString) ? formattedConnString : null;
    }


    /// <summary>
    ///   Gets or sets the server address to connect to
    /// </summary>
    [Category("Connection")]
    [DisplayName("Server")]
    [Description("Server address")]
    [DefaultValue("test-db-server")]
    [RefreshProperties(RefreshProperties.All)]
    public string Server
    {
      get { return GetPropertyValue("server"); }
      set { SetPropertyValue("server", value); }
    }

    /// <summary>
    ///   Gets or sets the broker port to connect to
    /// </summary>
    [Category("Connection")]
    [DisplayName("Port")]
    [Description("Broker port")]
    [DefaultValue("33000")]
    [RefreshProperties(RefreshProperties.All)]
    public string Port
    {
      get { return GetPropertyValue("port"); }
      set { SetPropertyValue("port", value); }
    }

    /// <summary>
    ///   Gets or sets the name of the database to connect to
    /// </summary>
    [Category("Connection")]
    [DisplayName("Database")]
    [Description("Database to connect to")]
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.All)]
    public string Database
    {
      get { return GetPropertyValue("database"); }
      set { SetPropertyValue("database", value); }
    }

    /// <summary>
    ///   Gets or sets the user id that should be used to connect with.
    /// </summary>
    [Category("Security")]
    [DisplayName("User")]
    [Description("Username")]
    [DefaultValue("public")]
    [RefreshProperties(RefreshProperties.All)]
    public string User
    {
      get { return GetPropertyValue("user"); }
      set { SetPropertyValue("user", value); }
    }

    /// <summary>
    ///   Gets/sets the database user password
    /// </summary>
    [Category("Security")]
    [DisplayName("Password")]
    [Description("User password for the database")]
    [Browsable(true)]
    [PasswordPropertyText(true)]
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.All)]
    public string Password
    {
      get { return GetPropertyValue("password"); }
      set { SetPropertyValue("password", value); }
    }

    /// <summary>
    ///   Gets/sets the database connection encoding
    /// </summary>
    [Category("Connection")]
    [DisplayName("Charset")]
    [Description("Connection echarset")]
    [DefaultValue("ASCII")]
    [RefreshProperties(RefreshProperties.All)]
    public string Encoding
    {
      get { return GetPropertyValue("charset"); }
      set { SetPropertyValue("charset", value); }
    }

    /// <summary>
    ///   Gets/sets the database connection auto-commit mode
    /// </summary>
    [Category("Connection")]
    [DisplayName("Auto-Commit")]
    [Description("Turn ON/OFF of auto-commit mode")]
    [DefaultValue("")]
    [RefreshProperties(RefreshProperties.All)]
    public string AutoCommit
    {
      get { return GetPropertyValue("autocommit"); }
      set { SetPropertyValue("autocommit", value); }
    }

    private string Initialize(string connString)
    {
      if (!String.IsNullOrEmpty(connString))
      {
        Dictionary<string, string> _connProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        // Parse connection string, Separator is(;)
        string _connString = "";
        string[] properties = connString.Split(';');
        //If the connection string ends with (;) remove last property
        if (properties[properties.Length - 1] == String.Empty)
          Array.Resize(ref properties, properties.Length-1);
        // Parse each values
        foreach (string p in properties)
        {
          string[] pair = p.Split('=');
          if (pair.Length == 2)
          {
            if (validKeywords.Contains(pair[0].ToLower()))
            {
              _connProperties.Add(pair[0].ToLower(), pair[1]);
            }
            else
            {
              throw new ArgumentException(Utils.GetStr(MsgId.InvalidConnectionString));
            }
          }
          else
          {
            throw new ArgumentException(Utils.GetStr(MsgId.InvalidConnectionString));
          }
          _connString += pair[0].ToLower() + "=" + pair[1] + ";";
        }

        // Update properties values
        foreach (KeyValuePair<String, String> p in _connProperties)
        {
          switch (p.Key)
          {
            case "server": // server
              Server = p.Value;
              break;
            case "port": // broker port
              Port = p.Value;
              break;
            case "database": // database name
              Database = p.Value;
              break;
            case "user": // database user
              User = p.Value;
              break;
            case "password": // user password
              Password = p.Value;
              break;
            case "charset": // Character set
              Encoding = p.Value;
              break;
            case "autocommit": // auto-commit
              AutoCommit = p.Value;
              break;
            default:
              throw new ArgumentException(Utils.GetStr(MsgId.InvalidPropertyName) + " : " + p.Key);
          }
        }

        return _connString;
      }

      return null;
    }

    /// <summary>
    ///   Gets the connection string.
    /// </summary>
    /// <returns> </returns>
    public string GetConnectionString()
    {
      return BuildConnStringFromProperties();
    }

    /// <summary>
    ///   Gets the value of a connection string property.
    /// </summary>
    /// <param name="key"> The name of the property. </param>
    /// <returns> </returns>
    private string GetPropertyValue(string key)
    {
      if (connProperties.ContainsKey(key))
      {
        return connProperties[key];
      }

      return null;
    }

    /// <summary>
    ///   Sets the value of a connection string property.
    /// </summary>
    /// <param name="key"> The name of the property. </param>
    /// <param name="value"> The value to set. </param>
    private void SetPropertyValue(string key, object value)
    {
      if (connProperties.ContainsKey(key))
      {
        connProperties[key] = value.ToString();
      }
      else
      {
        connProperties.Add(key, value.ToString());
      }
    }

    /// <summary>
    ///   Buils a connection string from its properties.
    /// </summary>
    private string BuildConnStringFromProperties()
    {
      StringBuilder connString = new StringBuilder();
      string delimiter = "";

      foreach (string p in connProperties.Keys)
      {
        connString.AppendFormat(CultureInfo.CurrentCulture, "{0}{1}={2}", delimiter, p, connProperties[p]);
        delimiter = ";";
      }

      return connString.ToString();
    }

    /// <summary>
    ///   Destroys a instance of the <see cref="CUBRIDConnectionStringBuilder" /> class.
    /// </summary>
    ~CUBRIDConnectionStringBuilder()
    {
      //Flush listeners
      foreach (TraceListener listener in Trace.Listeners)
      {
        listener.Flush();
      }
    }
  }
}