// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


const initializeSignalRConnection = (t) => {
    const conn = new signalR.HubConnectionBuilder()
        .withUrl("/trackerhub?tracker="+t)
        .build();

    conn.start().catch(err => console.error(err.toString()));
    return conn;
}

