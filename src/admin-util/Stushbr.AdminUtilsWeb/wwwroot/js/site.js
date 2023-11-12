// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class Items {

    static deleteItem(itemId) {
        fetch('/Items/DeleteItem/' + itemId, {
            method: 'DELETE',
            redirect: 'follow'
        })
            .then(response => {
                if (response.ok) {
                    if (response.redirected) {
                        window.location.href = response.url;
                    }
                } else {
                    alert('Error: ' + response.text());
                }
            });
    }
}