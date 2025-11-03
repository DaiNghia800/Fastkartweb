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
                data.forEach(function (sub) {
                    options += `<option value="${sub.uid}">${sub.subCategoryName}</option>`;
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

//bootstrap - tagsinput
$(document).ready(function () {
    $('input[data-role="tagsinput"]').tagsinput({
        trimValue: true,  
        confirmKeys: [13, 44] 
    });
});
//end bootstrap - tagsinput

//add option product variant
function clearValidation(element) {
    if (!element) return;

    const parentCol = element.closest('.col-9');
    const oldMessage = parentCol ? parentCol.querySelector('.validation-message') : null;
    if (oldMessage) {
        oldMessage.remove();
    }
    element.classList.remove('is-invalid');

    const select2Container = element.closest('.col-9')?.querySelector('.select2 .selection');
    if (select2Container) {
        select2Container.classList.remove('is-invalid');
    }

    if (element.getAttribute('data-role') === 'tagsinput') {
        const tagsinputDiv = parentCol ? parentCol.querySelector('.bootstrap-tagsinput') : null;
        if (tagsinputDiv) {
            tagsinputDiv.classList.remove('is-invalid');
        }
    }
}
function showInlineError(element, message) {
    let targetElement = element;

    if (element.getAttribute('data-role') === 'tagsinput') {

        const optionGroup = element.closest('.option-group'); 

        const tagsinputDiv = optionGroup ? optionGroup.querySelector('.bootstrap-tagsinput') : null;

        if (tagsinputDiv) {
            targetElement = tagsinputDiv;
        }
    }

    targetElement.classList.add('is-invalid');

    const errorMessage = document.createElement('div');
    errorMessage.className = 'validation-message';
    errorMessage.textContent = message;

    element.closest('.col-9').appendChild(errorMessage);

    targetElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
}

function isLastOptionGroupComplete() {
    const optionGroups = document.querySelectorAll('.option-group');
    const lastGroup = optionGroups[optionGroups.length - 1];

    const selectElement = lastGroup.querySelector('select');
    const valueInputElement = lastGroup.querySelector('input[data-role="tagsinput"]');

    clearValidation(selectElement);
    clearValidation(valueInputElement);

    if (selectElement.value === "") {
        const select2Container = lastGroup.querySelector('.select2 .selection');
        showInlineError(select2Container, "Vui lòng chọn Tên Tùy chọn.");
        return false;
    }

    let hasValues = false;
    if (typeof $ !== 'undefined' && $.fn.tagsinput) {
        const tags = $(valueInputElement).tagsinput('items');
        if (tags && tags.length > 0) {
            hasValues = true;
        }
    }

    if (!hasValues) {
        showInlineError(valueInputElement, "Vui lòng chọn Tên Tùy chọn.");
        valueInputElement.focus();
        return false;
    }

    return true; 
}

const buttonAddOption = document.querySelector("#button-add-option");
let optionCount = 1;
if (buttonAddOption) {
    const container = document.querySelector("#product-variations-container");
    const optionNamesData = JSON.parse(document.querySelector("[option-name-data]").getAttribute("option-name-data"));
    buttonAddOption.addEventListener("click", (event) => {
        event.preventDefault();

        if (!isLastOptionGroupComplete()) {
            return; 
        }

        const newOptionHtml = `<div id="option-group-${optionCount}" class="option-group">
                                    <div class="row align-items-center">
                                        <div class="col-11">
                                            <div class="row mb-4 align-items-center">
                                                <label for="optionName" class="col-3 form-label-title">Option Name ${optionCount}</label>
                                                <div class="col-9">
                                                    <select class="js-example-basic-single w-100" option-name id="optionName-select-${optionCount}" name="option_name">                                             
                                                        <option value="" selected>--Chọn một thuộc tính--</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="row mb-2 align-items-center">
                                                <label for="optionValue" class="col-3 form-label-title">Option Value ${optionCount}</label>
                                                <div class="col-9">
                                                    <input type="text" id="optionValue-input-${optionCount}" option-value name="option_name${optionCount}"  data-role="tagsinput" placeholder="Type tag & hit enter" />
                                                </div>
                                            </div> 
                                        </div>
                                        <div class="col-1">
                                            <a href="#" class="text-danger" data-index=${optionCount} button-remove-option>
                                                <i class="fa-solid fa-trash-can"></i>
                                            </a>
                                        </div>
                                    </div>        
                                </div>
                            `;
        container.insertAdjacentHTML("beforeend", newOptionHtml);

        const selectElement = document.getElementById(`optionName-select-${optionCount}`);

        optionNamesData.forEach(optionName => {
            const option = document.createElement('option');
            option.value = optionName.uid;
            option.textContent = optionName.name;
            selectElement.appendChild(option);
        });

        if (selectElement) {
            $(selectElement).select2();
        }

        const inputElement = document.querySelector(`#optionValue-input-${optionCount}`);
        if (inputElement) {
            $(inputElement).tagsinput('refresh'); 
        }

        optionCount++;

    });
}
//end add option product variant

//remove option product variant
function updateOptionIndices() {
    const optionGroups = document.querySelectorAll('#product-variations-container > .option-group');

    const optionGroupArray = Array.from(optionGroups).slice(1);

    let currentIndex = 1;

    optionGroupArray.forEach(group => {
        group.id = `option-group-${currentIndex}`;

        const optionNameLabel = group.querySelectorAll('.form-label-title');
        if (optionNameLabel.length > 0) {
            optionNameLabel.forEach(option => {
                option.textContent = `Option Name ${currentIndex}`;
            })
        }

        const selectElement = group.querySelector('select');
        if (selectElement) {
            selectElement.name = `option_name[${currentIndex - 1}].Name`;
            selectElement.id = `optionName-select-${currentIndex}`;
            $(selectElement).select2();
        }

        const valueInputElement = group.querySelector('input[data-role="tagsinput"]');
        if (valueInputElement) {
            valueInputElement.name = `option_name[${currentIndex - 1}].Values`;
            valueInputElement.id = `optionValue-input-${currentIndex}`;
        }

        const removeButton = group.querySelector('[button-remove-option]');
        if (removeButton) {
            removeButton.setAttribute('data-index', currentIndex);
        }

        currentIndex++;
    });

    optionCount = currentIndex;
}

document.addEventListener("DOMContentLoaded", function () {
    const container = document.querySelector("#product-variations-container");

    if (container) {
        container.addEventListener("click", (event) => {
            const button = event.target.closest("[button-remove-option]");

            if (button) {
                event.preventDefault(); 

                const dataIndex = button.getAttribute("data-index");

                if (dataIndex) {
                    const elementRemove = document.querySelector(`#option-group-${dataIndex}`);

                    if (elementRemove) {
                        elementRemove.remove();
                        updateOptionIndices();
                    }
                }
            }
        });
    }
});

//end remove option product variant

// binding product variant
const optionValueCache = {};
async function getOptionValues(optionNameId) {
    if (optionValueCache[optionNameId]) return optionValueCache[optionNameId];

    const res = await fetch(`/admin/product/optionValue/${optionNameId}`);
    const data = await res.json();
    optionValueCache[optionNameId] = data;

    return data;
}

$(document).on("change", "[option-name]", async function () {
    const $group = $(this).closest(".option-group");
    const $input = $group.find("[option-value]");
    const optionNameId = $(this).val();
    const $selection = $group.find(".select2 .selection");
    const $listOptionName = $("[option-name]").not(this);
    const isDuplicate = $listOptionName.toArray().some(el => $(el).val() === optionNameId);
    if (isDuplicate) {
        clearValidation($selection[0]);
        showInlineError($selection[0], "Thuộc tính này đã tồn tại.");
        return;
    }

    $input.tagsinput("removeAll");
    $input.data("validValues", []);

    if (!optionNameId) return;
    clearValidation($selection[0]);
    clearValidation($input[0]);
    const values = await getOptionValues(optionNameId);
    $input.data("validValues", values);
    var availableTags = ['JavaScript', 'Java', 'Python', 'C#', 'PHP', 'HTML', 'CSS', 'React', 'Vue'];
    $(document).ready(function () {
        const availableTags = ['JavaScript', 'Java', 'Python', 'C#', 'PHP', 'HTML', 'CSS', 'React', 'Vue'];

        const $input = $('#optionValue');
        const $suggestions = $('#suggestions');

        // Khởi tạo tagsinput
        $input.tagsinput({
            trimValue: true,
            confirmKeys: [13, 44] // Enter hoặc dấu phẩy
        });

        // Hiển thị gợi ý khi nhập
        $input.on('input', function () {
            const val = $(this).val().toLowerCase();
            $suggestions.empty();

            if (!val) {
                $suggestions.hide();
                return;
            }

            // Lọc các tag chưa thêm
            const existingTags = $input.tagsinput('items');
            const matches = availableTags.filter(tag =>
                tag.toLowerCase().includes(val) && !existingTags.includes(tag)
            );

            if (matches.length === 0) {
                $suggestions.hide();
                return;
            }

            matches.forEach(tag => {
                $suggestions.append(`<button type="button" class="list-group-item list-group-item-action">${tag}</button>`);
            });

            $suggestions.show();
        });

        // Khi click vào gợi ý
        $suggestions.on('click', 'button', function () {
            const tag = $(this).text();
            $input.tagsinput('add', tag);
            $input.val('');
            $suggestions.hide();
        });

        // Ẩn gợi ý khi click ngoài
        $(document).on('click', function (e) {
            if (!$(e.target).closest('.option-group').length) {
                $suggestions.hide();
            }
        });
    });
});

$(document).on("beforeItemAdd", "[option-value]", function (event) {
    const $optionGroup = $(this).closest(".option-group");
    const $optionName = $optionGroup.find("[option-name]");
    const $selection = $optionGroup.find(".select2 .selection");
    if ($optionName.val() == "") {
        event.cancel = true;
        clearValidation($selection[0]);
        showInlineError($selection[0], "Vui lòng chọn Tên Tùy chọn.");
        return;
    }
    const validValues = ($(this).data("validValues") || []).map(v => v.trim().toLowerCase());
    const newValue = String(event.item).trim().toLowerCase();
    if (!validValues.includes(newValue)) {
        event.cancel = true;
        clearValidation($(this)[0]);
        showInlineError($(this)[0], `${event.item} không hợp lệ cho thuộc tính này.`);
    } else {
        clearValidation($(this)[0]);
    }
})
// end binding product variant