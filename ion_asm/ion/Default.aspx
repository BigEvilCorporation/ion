<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ion._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>ion::code</h2>
            </hgroup>
            <p>
                A teeny tiny assembly language for making teeny tiny 2D games</p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:TextBox ID="txtSourceCode" runat="server" Height="450px" MaxLength="140" OnTextChanged="txtEditor_TextChanged" TextMode="MultiLine" Width="733px"></asp:TextBox>
    <asp:GridView ID="gridMemory" runat="server" OnSelectedIndexChanged="gridMemory_SelectedIndexChanged" ShowHeader="False">
    </asp:GridView>
    <asp:Button ID="btnCompile" runat="server" OnClick="btnCompile_Click" Text="Compile" />
    <asp:Button ID="btnExecute" runat="server" OnClick="btnExecute_Click" Text="Execute" />
</asp:Content>
