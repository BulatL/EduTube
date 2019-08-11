﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var searchInput = document.getElementById("searchInput");
    searchInput.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            let search_query = searchInput.value.replace(/ /g, '-');
            event.preventDefault();
            window.location.href = "/Search?search_query=" + search_query;
        }
    });
})