// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    sidebarToggle();
});


function sidebarToggle() {
    const sidebar = document.getElementsByClassName("js-sidebar")[0];
    const toggle = document.getElementsByClassName("js-sidebar-toggle")[0];

    if (!sidebar || !toggle) return;

    toggle.addEventListener("click", function () {
        sidebar.classList.toggle("collapsed");

        sidebar.addEventListener(
            "transitionend",
            function () {
                window.dispatchEvent(new Event("resize"));
            },
            { once: true } // prevents multiple event stacking
        );
    });
}