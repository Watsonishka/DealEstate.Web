// =======================================================
// Чекбокс "Нет отчества"
// =======================================================
document.addEventListener('DOMContentLoaded', function () {

    const hasPatronymicCheckbox = document.getElementById('hasPatronymic');
    const patronymicInput = document.getElementById('patronymicInput');

    if (!hasPatronymicCheckbox || !patronymicInput)
        return;

    function updatePatronymic() {
        if (hasPatronymicCheckbox.checked) {
            patronymicInput.disabled = true;
            patronymicInput.value = '';
        } else {
            patronymicInput.disabled = false;
        }
    }

    hasPatronymicCheckbox.addEventListener('change', updatePatronymic);
    updatePatronymic();
});


// =======================================================
// Переключение полей для квартиры/дома
// =======================================================
document.addEventListener('DOMContentLoaded', function () {

    const typeSelect = document.getElementById('propertyType');
    const apartmentFields = document.getElementById('apartment-fields');
    const houseFields = document.getElementById('house-fields');
    const productForm = document.getElementById('productForm');

    if (!typeSelect) return;

    function updateFields() {
        const selected = typeSelect.value;
        const isApartment = selected === 'Apartments';
        const isHouse = selected === 'Houses';

        if (apartmentFields) {
            apartmentFields.style.display = isApartment ? 'block' : 'none';
        }

        if (houseFields) {
            houseFields.style.display = isHouse ? 'block' : 'none';
        }

        if (window.jQuery && productForm) {
            const $form = jQuery(productForm);
            const validator = $form.data('validator');

            if (validator) {
                if (!isApartment && apartmentFields) {
                    // Если переключили на Дом, убираем ошибки с полей Квартиры
                    clearContainerValidation(apartmentFields, validator);
                }
                if (!isHouse && houseFields) {
                    // Если переключили на Квартиру, убираем ошибки с полей Дома
                    clearContainerValidation(houseFields, validator);
                }
            }
        }
    }

    // Вспомогательная функция, которая мягко чистит ошибки только в скрытом контейнере
    function clearContainerValidation(container, validator) {
        const inputs = container.querySelectorAll('input, select, textarea');
        inputs.forEach(input => {
            // Удаляем класс ошибки с инпута
            input.classList.remove('input-validation-error');

            // Заставляем jQuery Validate забыть про ошибку этого поля
            if (validator.submitted && validator.submitted[input.name]) {
                delete validator.submitted[input.name];
            }

            // Находим и прячем спан с текстом ошибки
            const errorSpan = document.querySelector(`[data-valmsg-for="${input.name}"]`);
            if (errorSpan) {
                errorSpan.classList.remove('field-validation-error');
                errorSpan.classList.add('field-validation-valid');
                errorSpan.innerHTML = '';
            }
        });
    }

    updateFields();
    typeSelect.addEventListener('change', updateFields);
});


// =======================================================
// Режим редактирования недвижимости (кнопка "Редактировать"/"Сохранить")
// =======================================================
document.addEventListener('DOMContentLoaded', function () {

    const editToggleBtn = document.getElementById('editToggleBtn');
    const saveBtn = document.getElementById('saveBtn');
    const editableFields = document.querySelectorAll('.editable-field');

    if (!editToggleBtn) return;

    function toggleEditMode(enable) {
        editableFields.forEach(function (field) {
            if (field.tagName === 'SELECT' || field.tagName === 'INPUT' || field.tagName === 'TEXTAREA') {
                field.disabled = !enable;
            }
            if (field.tagName === 'INPUT' && field.type === 'checkbox') {
                field.disabled = !enable;
            }
        });

        if (enable) {
            // Режим редактирования: кнопка "Редактировать" неактивна, "Сохранить" активна
            editToggleBtn.disabled = true;
            editToggleBtn.classList.add('opacity-50');
            if (saveBtn) {
                saveBtn.classList.remove('d-none');
                saveBtn.disabled = false;
            }
        } else {
            // Режим просмотра: кнопка "Редактировать" активна, "Сохранить" скрыта
            editToggleBtn.disabled = false;
            editToggleBtn.classList.remove('opacity-50');
            if (saveBtn) {
                saveBtn.classList.add('d-none');
                saveBtn.disabled = true;
            }
        }
    }

    editToggleBtn.addEventListener('click', function () {
        var isEnabled = editableFields.length > 0 && !editableFields[0].disabled;
        toggleEditMode(!isEnabled);
    });

    // Если есть ошибки валидации — включаем режим редактирования
    var hasValidationErrors = document.querySelector('.field-validation-error') !== null;

    if (hasValidationErrors) {
        toggleEditMode(true);
    } else {
        toggleEditMode(false);
    }
});

// =======================================================
// Карусель на главной странице
// =======================================================

document.addEventListener("DOMContentLoaded", function () {

    const carousel = document.getElementById("catalogHero");

    function restartAnimation() {

        const active = carousel.querySelector(".carousel-item.active .hero-image");

        if (!active) return;

        active.style.animation = "none";
        active.offsetHeight;
        active.style.animation = "";

    }

    restartAnimation();

    carousel.addEventListener("slid.bs.carousel", restartAnimation);

});

// =======================================================
// Редактирование карточки товара
// =======================================================


$(document).ready(function () {
    const editToggleBtn = document.getElementById('editToggleBtn');
    const saveBtn = document.getElementById('saveBtn');
    const fileUploadSection = document.getElementById('fileUploadSection');
    const isEditMode = document.getElementById('IsEditMode');

    // Функция обновления видимости
    function updateEditMode(editMode) {
        const editableFields = document.querySelectorAll('.editable-field');
        const fileInputs = document.querySelectorAll('.editable-field[type="file"]');

        // Обновляем все поля
        editableFields.forEach(field => {
            field.disabled = !editMode;
        });

        // Специальная обработка для файловых полей
        fileInputs.forEach(input => {
            input.disabled = !editMode;
        });

        // Показываем/скрываем секцию загрузки файла
        if (fileUploadSection) {
            if (editMode) {
                fileUploadSection.classList.remove('edit-mode-hidden');
                fileUploadSection.classList.add('edit-mode-visible');
            } else {
                fileUploadSection.classList.remove('edit-mode-visible');
                fileUploadSection.classList.add('edit-mode-hidden');
                // Сбрасываем значение файла при выходе из режима редактирования
                const fileInput = document.getElementById('uploadedFileInput');
                if (fileInput) {
                    fileInput.value = '';
                    document.getElementById('fileNameDisplay').textContent = 'Файл не выбран';
                }
            }
        }

        // Показываем/скрываем кнопку "Сохранить"
        if (saveBtn) {
            saveBtn.classList.toggle('d-none', !editMode);
            saveBtn.disabled = !editMode;
        }

        // Меняем текст кнопки
        if (editToggleBtn) {
            editToggleBtn.textContent = editMode ? 'Отмена' : 'Редактировать';
            if (editMode) {
                editToggleBtn.classList.add('btn-danger');
                editToggleBtn.classList.remove('btn-main');
            } else {
                editToggleBtn.classList.remove('btn-danger');
                editToggleBtn.classList.add('btn-main');
            }
        }

        // Обновляем скрытое поле
        if (isEditMode) {
            isEditMode.value = editMode ? 'true' : 'false';
        }
    }

    // Обработчик кнопки "Редактировать"
    if (editToggleBtn) {
        editToggleBtn.addEventListener('click', function () {
            const isCurrentlyEditMode = isEditMode ? isEditMode.value === 'true' : false;
            // Переключаем режим
            updateEditMode(!isCurrentlyEditMode);
        });
    }

    // Функция обновления имени файла
    window.updateFileName = function (input) {
        const fileNameDisplay = document.getElementById('fileNameDisplay');
        if (input.files && input.files.length > 0) {
            fileNameDisplay.textContent = input.files[0].name;
        } else {
            fileNameDisplay.textContent = 'Файл не выбран';
        }
    };

    // Инициализация - по умолчанию режим просмотра
    updateEditMode(false);
});