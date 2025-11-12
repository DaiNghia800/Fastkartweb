//const { data } = require("jquery");

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

//dropzone
const dropzoneElement = document.querySelector("#my-dropzone");
if (dropzoneElement) {
    Dropzone.autoDiscover = false;
    let uploadedImages = [];
    const thumbnail = document.getElementById("Thumbnail");
    let dataThumbnail;
    if (thumbnail) {
        dataThumbnail = thumbnail.getAttribute("data-thumbnail");
    }

    const myDropzone = new Dropzone(dropzoneElement, {
        url: "/admin/upload/image",
        method: "post",
        paramName: 'files',
        autoProcessQueue: false,
        uploadMultiple: true, 
        parallelUploads: 10,
        maxFilesize: 5,
        acceptedFiles: "image/*",
        addRemoveLinks: true,
        headers: {
            "Cache-Control": null,
            "X-Requested-With": null  
        },
        dictDefaultMessage: `
            <div class= "dz-message-inner">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-upload-cloud"><polyline points="16 16 12 12 8 16"></polyline><line x1="12" y1="12" x2="12" y2="21"></line><path d="M20.39 18.39A5 5 0 0 0 18 9h-1.26A8 8 0 1 0 3 16.3"></path><polyline points="16 16 12 12 8 16"></polyline></svg>
                <p>Thả file vào đây hoặc click để tải lên</p>
            </div >
        `,

        init: function () {
            var dz = this;

            if (dataThumbnail) {
                try {
                    uploadedImages = JSON.parse(dataThumbnail);
                } catch (e) {
                    uploadedImages = [];
                }
            }

            const buttonSave = document.querySelector("[button-save]");
            if (buttonSave) {
                buttonSave.addEventListener("click", function (e) {
                    e.preventDefault();

                    if (dz.getQueuedFiles().length > 0) {
                        dz.processQueue();
                    } else {
                        document.getElementById("Thumbnail").value = JSON.stringify(uploadedImages);
                        document.getElementById("mainForm").submit();
                    }
                });
            }

            this.on("sending", function (file, xhr, formData) {
                formData.append("files", file);
            });

            if (dataThumbnail !== null) {
                uploadedImages.forEach(function (url) {
                    var mockFile = { name: url.split("/").pop(), size: 12345, existingUrl: url };
                    dz.emit("addedfile", mockFile);
                    dz.emit("thumbnail", mockFile, url);
                    dz.emit("complete", mockFile);
                });
            }

            this.on("successmultiple", function (file, response) {
                if (response.urls) {
                    uploadedImages.push(...response.urls);
                } else if (response.url) {
                    uploadedImages.push(response.url);
                }
                document.getElementById("Thumbnail").value = JSON.stringify(uploadedImages);
                document.getElementById("mainForm").submit();
            });

            this.on("error", function (file, message) {
                console.error("Upload failed:", message);
            });
            this.on("removedfile", function (file) {
                let url = file.xhr ? JSON.parse(file.xhr.response).url : file.existingUrl || file.dataUrl;

                if (!url) return;

                const index = uploadedImages.indexOf(url);
                if (index > -1) {
                    uploadedImages.splice(index, 1);
                    document.getElementById("Thumbnail").value = JSON.stringify(uploadedImages);
                }
            });
        }
    });
}
//end dropzone

// get subcategory by category
var subcategorySelect = $("#subcategory");
var categorySelect = $("#category");
function getSubCategory(categoryId) {
    $.ajax({
        url: "/admin/product/subCategory",
        type: "GET",
        dataType: 'json',
        data: { categoryId: categoryId },
        success: function (data) {
            if (data.length > 0) {
                var options;
                const subcategoryId = subcategorySelect[0].getAttribute("subcategory-id");
                data.forEach(function (sub) {
                    options += `<option value="${sub.uid}" ${sub.uid == subcategoryId ? "selected" : ""}>${sub.subCategoryName}</option>`;
                });
                subcategorySelect.html(options).trigger('change');
            } else {
                subcategorySelect.html('<option disabled selected value="">Không có SubCategory</option>').trigger('change');
            }
        },
        error: function (err) {
            subcategorySelect.html('<option disabled selected value="">Không có SubCategory</option>').trigger('change');
            console.error(err);
        }
    });
}

getSubCategory(categorySelect.val());
categorySelect.on("change", function () {
    getSubCategory($(this).val());
})
// end get subcategory by category

//delete
const listButtonDelete = document.querySelectorAll("[button-delete]");
if (listButtonDelete.length) {
    listButtonDelete.forEach(buttonDelete => {
        buttonDelete.addEventListener("click", (event) => {
            event.preventDefault();

            Swal.fire({
                title: "Bạn có chắc chắn muốn xóa bảng ghi này?",
                text: "",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Đồng ý",
                cancelButtonText: "Huỷ bỏ",
            }).then((result) => {
                if (result.isConfirmed) {
                    const patch = buttonDelete.getAttribute("data-patch");
                    const id = buttonDelete.getAttribute("button-id");

                    const data = {
                        id: id
                    }

                    fetch(patch, {
                        headers: {
                            "Content-Type": "application/json",
                        },
                        method: "POST",
                        body: JSON.stringify(data)
                    })
                        .then(res => res.json())
                        .then(data => {
                            if (data.code == "success") {
                                Swal.fire({
                                    title: "Đã xóa!",
                                    text: "",
                                    icon: "success",
                                    timer: 1500,
                                    showConfirmButton: false
                                }).then(() => {
                                    location.reload();
                                });
                            }
                        })
                }
            });

        })
    })
}
//end delete

//change status
const listButtonChangeStatus = document.querySelectorAll("[button-change-status]");

if (listButtonChangeStatus.length > 0) {
    listButtonChangeStatus.forEach(button => {
        button.addEventListener("click", () => {
            const itemId = button.getAttribute("item-id");
            const statusChange = button.getAttribute("button-change-status");
            const patch = button.getAttribute("data-patch");

            const data = {
                id: itemId,
                status: statusChange
            }

            fetch(patch, {
                headers: {
                    "Content-Type": "application/json",
                },
                method: "POST",
                body: JSON.stringify(data)
            })
                .then(res => res.json())
                .then(data => {
                    if (data.code == "success") {
                        location.reload();
                    }
                })
        })
    })
}
//end change status

//pagination
const listButtonPagination = document.querySelectorAll("[button-pagination]");
if (listButtonPagination.length > 0) {
    let url = new URL(location.href);
    listButtonPagination.forEach(button => {
        button.addEventListener("click", (event) => {
            event.preventDefault();

            const page = button.getAttribute("button-pagination");
            if (page) {
                url.searchParams.set("page", page);
            } else {
                url.searchParams.delete("page");
            }

            location.href = url.href
        })
    })
}
//end pagination

//filter
const boxFilter = $("[box-filter]");
if (boxFilter.length > 0) {
    let url = new URL(location.href);
    boxFilter.on("change", function () {
        const value = boxFilter.val();

        if (value) {
            url.searchParams.set("status", value);
        } else {
            url.searchParams.delete("status");
        }

        location.href = url.href;
    });

    //display default
    const statusCurrent = url.searchParams.get("status");
    if (statusCurrent) {
        boxFilter.val(statusCurrent);
    }
    //end display default
}
//end filter

//sort
const sortSelect = $("[sort-select]");
if (sortSelect.length > 0) {
    let url = new URL(location.href);
    sortSelect.on("change", function () {
        const value = sortSelect.val();

        if (value) {
            const [sortKey, sortValue] = value.split("-");
            url.searchParams.set("sortKey", sortKey);
            url.searchParams.set("sortValue", sortValue);
        } else {
            url.searchParams.delete("sortKey");
            url.searchParams.delete("sortValue");
        }

        location.href = url.href;
    });

    //display default
    const sortKeyCurrent = url.searchParams.get("sortKey");
    const sortValueCurrent = url.searchParams.get("sortValue");
    if (sortKeyCurrent && sortValueCurrent) {
        sortSelect.val(`${sortKeyCurrent}-${sortValueCurrent}`);
    }
    //end display default
}
//end sort

//changemulti
const formChangeMulti = document.querySelector("[form-change-multi]");
if (formChangeMulti) {
    formChangeMulti.addEventListener("submit", (event) => {
        event.preventDefault();

        const patch = formChangeMulti.getAttribute("data-patch");
        const status = formChangeMulti.status.value;

        if (status == "delete") {
            Swal.fire({
                title: "Bạn có chắc chắn muốn xóa những bảng ghi này?",
                text: "",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Đồng ý",
                cancelButtonText: "Huỷ bỏ",
            }).then((result) => {
                if (!result.isConfirmed) {
                    return;
                }
                submitChangeMulti();
            });
        } else {
            submitChangeMulti();
        }

        function submitChangeMulti() {
            const ids = [];

            const listInputChange = document.querySelectorAll("[input-change]:checked");

            listInputChange.forEach(input => {
                const id = input.getAttribute("input-change");
                ids.push(id);
            });

            const data = {
                id: ids,
                status: status
            };

            fetch(patch, {
                headers: {
                    "Content-Type": "application/json",
                },
                method: "POST",
                body: JSON.stringify(data)
            })
                .then(res => res.json())
                .then(data => {
                    if (data.code == "deleted") {
                        Swal.fire({
                            title: "Xóa thành công!",
                            text: "",
                            icon: "success",
                            timer: 1500,
                            showConfirmButton: false
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        location.reload();
                    }
                })
        }
    })
}
//end changemulti

//change position
const listInputPosition = document.querySelectorAll("[input-position]");
if (listInputPosition.length > 0) {
    listInputPosition.forEach(input => {
        input.addEventListener("change", () => {
            const value = parseInt(input.value)
            const id = parseInt(input.getAttribute("item-id"));
            const patch = input.getAttribute("data-patch");

            fetch(patch, {
                headers: {
                    "Content-Type": "application/json",
                },
                method: "POST",
                body: JSON.stringify({
                    id: id,
                    position: value
                })
            })
                .then(res => res.json())
                .then(data => {
                    if (data.code == "success") {
                        location.reload();
                    }
                })
        })
    })
}
//end change position

//permission
const tablePermission = document.querySelector("[table-permission]");
if (tablePermission) {
    const buttonSubmit = document.querySelector("[button-submit]");
    if (buttonSubmit) {
        buttonSubmit.addEventListener("click", () => {
            const data = [];

            const listElementRoleId = document.querySelectorAll("[role-id]");
            listElementRoleId.forEach(elementRoleId => {
                const roleId = elementRoleId.getAttribute("role-id");
                const permission = [];
                const listInputChecked = document.querySelectorAll(`input[data-id="${roleId}"]:checked`);

                listInputChecked.forEach(input => {
                    const tr = input.closest("tr[data-name]");
                    const name = tr.getAttribute("data-name");

                    permission.push(name);

                });

                data.push({
                    id: roleId,
                    permission: permission
                });
            });

            const patch = buttonSubmit.getAttribute("data-patch");
            fetch(patch, {
                headers: {
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: JSON.stringify(data)
            })
                .then(res => res.json())
                .then(data => {
                    if (data.code == "success") {
                        location.reload();
                    }
                })
        });
    }
    //display default
    let dataPermission = tablePermission.getAttribute("table-permission");
    dataPermission = JSON.parse(dataPermission)
    dataPermission.forEach(item => {
        item.permissions.forEach(permission => {
            const input = document.querySelector(`tr[data-name="${permission}"] input[data-id="${item.id}"]`);
            input.checked = true;
        })
    })
    //end display default
}
//end permission

//search All user
$(document).ready(function () {
    $("#searchInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#userTableBody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
//end search All user

//show user detail 
$(document).ready(function () {
    $('#userDetailModal').on('show.bs.modal', function (event) {
        const button = $(event.relatedTarget);
        const userId = button.data('user-id');
        const contentDiv = $('#userDetailContent');

        // Hiển thị loading
        contentDiv.html(`
            <div class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Đang tải...</span>
                </div>
            </div>
        `);
        $.ajax({
            url: '/admin/user/get-user-detail',
            type: 'GET',
            data: { id: userId },
            success: function (responseHtml) {
                contentDiv.html(responseHtml);
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi khi tải dữ liệu",
                    text: res.message
                });
            }
        });
    });
});
//end show user detail

//show user edit 
$(document).ready(function () {
    $('#userEditModal').on('show.bs.modal', function (event) {
        const button = $(event.relatedTarget);
        const userId = button.data('user-id');
        const contentDiv = $('#userEditContent');

        // Hiển thị loading
        contentDiv.html(`
            <div class="text-center py-4">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Đang tải...</span>
                </div>
            </div>
        `);

        $.ajax({
            url: '/admin/user/get-user-edit',
            type: 'GET',
            data: { id: userId },
            success: function (responseHtml) {
                contentDiv.html(responseHtml);
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi khi tải dữ liệu",
                    text: "Không thể tải form chỉnh sửa"
                });
            }
        });
    });

    // Xử lý submit form edit
    $(document).on('submit', '#userEditForm', function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        $.ajax({
            url: '/admin/user/update',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Thành công",
                        text: response.message || "Cập nhật thông tin thành công"
                    }).then(() => {
                        $('#userEditModal').modal('hide');
                        location.reload(); // Reload trang để cập nhật dữ liệu
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Lỗi",
                        text: response.message || "Có lỗi xảy ra"
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: "Không thể cập nhật thông tin"
                });
            }
        });
    });
});
//end show user edit

//delete Users
// (Bên dưới code submit form edit)

// Xử lý sự kiện nhấn nút Xóa
$(document).on('click', '.icon-delete', function (e) {
    e.preventDefault(); // Ngăn hành vi mặc định của thẻ <a>

    const button = $(this);
    const userId = button.data('user-id');

    Swal.fire({
        title: 'Bạn có chắc không?',
        text: "Bạn sẽ không thể hoàn tác hành động này!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Vâng, hãy xóa nó!',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: '/admin/user/delete',
                type: 'POST',
                data: { id: userId }, // Dữ liệu gửi đi
                success: function (response) {
                    if (response.success) {
                        Swal.fire(
                            'Đã xóa!',
                            response.message,
                            'success'
                        );
                        // Xóa hàng <tr> cha của nút vừa bấm
                        button.closest('tr').fadeOut(500, function () {
                            $(this).remove();
                        });
                    } else {
                        Swal.fire(
                            'Lỗi!',
                            response.message,
                            'error'
                        );
                    }
                },
                error: function () {
                    Swal.fire(
                        'Lỗi!',
                        'Không thể kết nối đến máy chủ.',
                        'error'
                    );
                }
            });
        }
    });
});
//end delete Users

//My profile (admin)
function viewMyProfile(userId){
    $.ajax({
        url: '/admin/user/get-user-detail',
        type: 'GET',
        data: { id: userId },
        success: function (response) { 
            $('#myProfileContent').html(response);
            $('#myProfileModal').modal('show');
        },
        error: function(xhr, status, error){
            console.error('Error loading profile:', error);
            Swal.fire({
                icon: "error",
                title: "Lỗi khi tải dữ liệu",
                text: "Không thể tải thông tin cá nhân"
            });
        }
    });
}
//End my profile (admin)    
