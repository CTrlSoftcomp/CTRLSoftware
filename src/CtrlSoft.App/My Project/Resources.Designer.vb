﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("CtrlSoft.App.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property addnewdatasource_32x32() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("addnewdatasource_32x32", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Background() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Background", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property connect_to_database() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("connect_to_database", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property database() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("database", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property database_32x32() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("database_32x32", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property export_database2() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("export_database2", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property import_database2() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("import_database2", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to USE [master];
        '''GO
        '''
        '''/****** Object:  Database [DBPOS]    Script Date: 6/23/2021 11:20:17 AM ******/
        '''
        '''CREATE DATABASE [DBPOS] ON PRIMARY
        '''(
        '''                                   NAME = N&apos;DBPOS&apos;, 
        '''                                   FILENAME = N&apos;C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\DBPOS_data.mdf&apos;, 
        '''                                   SIZE = 5440 KB, 
        '''                                   MAXSIZE = UNLIMITED, 
        '''                                   FILEGROWTH = 1024 KB
        ''') LOG ON [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Migration() As String
            Get
                Return ResourceManager.GetString("Migration", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property redo_32x32() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("redo_32x32", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to -- Method 2 Isi Data Default
        '''USE [DBPOS]
        '''GO
        '''
        '''--
        '''-- Inserting data into table dbo.MAlamat
        '''--
        '''INSERT [DBPOS].dbo.MAlamat(NoID, Kode, Nama, NamaAlias, Alamat, Kota, HP, Telp, ContactPerson, LimitHutang, LimitPiutang, LimitNotaPiutang, LimitUmurPiutang, IDTypeHarga, IsActive, IsSupplier, IsPegawai, IsCustomer) VALUES (1, N&apos;KRY-00001&apos;, N&apos;ADMIN&apos;, N&apos;ADMIN&apos;, N&apos;-&apos;, N&apos;SURABAYA&apos;, N&apos;-&apos;, N&apos;-&apos;, N&apos;ADMIN&apos;, 0.00, 0.00, 0, 0, 0, 1, 0, 1, 0)
        '''INSERT [DBPOS].dbo.MAlamat(NoID, Kode, Nama, NamaAlias, Alamat, Kota, HP, Tel [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Seeder() As String
            Get
                Return ResourceManager.GetString("Seeder", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property technology_32x32() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("technology_32x32", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property undo_32x32() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("undo_32x32", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
