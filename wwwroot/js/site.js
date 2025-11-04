//according menu
const sidebarLink = document.querySelectorAll(".sidebar .sidebar-link");
if (sidebarLink.length > 0) {
    sidebarLink.forEach(item => {
        item.addEventListener("click", () => {
            const itemActive = document.querySelector(".sidebar .sidebar-link.active");
            if (itemActive) {
                itemActive.classList.remove("active");
            }

            const iconAccording = item.querySelector(".according-menu");
            if (iconAccording) {
                iconAccording.classList.toggle("active");
            }

            item.classList.add("active");
        })
    })
}
//end according menu

//close sidebar 
const buttonClose = document.querySelector(".toggle-sidebar");
if (buttonClose) {
    buttonClose.addEventListener("click", () => {
        const header = document.querySelector(".header");
        const sidebar = document.querySelector(".sidebar");

        header.classList.toggle("close");
        sidebar.classList.toggle("close");
    })
}
//end close sidebar

//slick
$(document).ready(function () {
    $('.slide-dashboard-category').slick({
        slidesToShow: 10,
        slidesToScroll: 1,
    });
});
//end slick

//select2
$(document).ready(function () {
    $('.js-example-basic-single').select2({
        width: 'resolve'
    });
});
//end select2

//tinymce 
tinymce.init({
    selector: '#editor',
    license_key: 'gpl',
    plugins: 'lists link image table code wordcount',
    toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright | code',
    height: 300,
    branding: true
});
//end tinymce


const dropzoneElement = document.querySelector("#my-dropzone");
if (dropzoneElement) {
    Dropzone.autoDiscover = false;
    const myDropzone = new Dropzone(dropzoneElement, {
        url: "javascript:void(0);",
        maxFilesize: 5,
        acceptedFiles: ".jpg,.png,.pdf",
        addRemoveLinks: true,
        dictDefaultMessage: `
            <div class= "dz-message-inner">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-upload-cloud"><polyline points="16 16 12 12 8 16"></polyline><line x1="12" y1="12" x2="12" y2="21"></line><path d="M20.39 18.39A5 5 0 0 0 18 9h-1.26A8 8 0 1 0 3 16.3"></path><polyline points="16 16 12 12 8 16"></polyline></svg>
                <p>Thả file vào đây hoặc click để tải lên</p>
            </div >
        `,

        init: function () {
            this.on("success", function (file, response) {
                console.log("Upload thành công:", response);
            });
            this.on("error", function (file, message) {
                console.log("Lỗi:", message);
            });
        }
    });
}