class FilmService {
    constructor(filmId, allCategories, selectedCategories) {
        this.filmId = filmId;
        this.allCategories = allCategories;
        this.selectedCategories = selectedCategories;
        //this.initCategorySelect();
    }

    initCategorySelect() {
        $('#categoriesSelect').select2({
            width: '100%',
            multiple: true,
            data: this.allCategories.map(category => ({ id: category.CategoryId, text: category.Name })),
            placeholder: 'Select categories'
        });

        // Set the selected categories in the dropdown
        $('#categoriesSelect').val(this.selectedCategories.map(category => category.CategoryId));
        $('#categoriesSelect').trigger('change');
    }

    saveCategories() {
        var selectedCategories = $('#categoriesSelect').val();

        $.ajax({
            url: `/Films/UpdateFilmCategories?filmId=${this.filmId}`,
            method: 'POST',
            data: {
                filmId: this.filmId,
                categoryIds: selectedCategories
            },
            success: function (response) {
                location.reload();
            },
            error: function (error) {
                console.error('Error updating categories:', error);
            }
        });
    }

    updateFilm(filmName, filmDirector, releaseDate) {
        $.ajax({
            url: '/Films/UpdateFilm?filmId=${this.filmId}',
            method: 'POST',
            data: {
                filmId: this.filmId,
                filmName: filmName,
                filmDirector: filmDirector,
                releaseDate: releaseDate
            },
            success: function (response) {
                // Reload the page to reflect the updated details
                location.reload();
            },
            error: function (error) {
                console.error('Error updating film details:', error);
            }
        });
    }

    deleteFilm() {
        $.ajax({
            url: '/Films/DeleteFilm?filmId=${this.filmId}',
            method: 'POST',
            data: {
                filmId: this.filmId
            },
            success: function (response) {
                window.location.href = '/Films/Index'; // Redirect to films index page
            },
            error: function (error) {
                console.error('Error deleting film:', error);
            }
        });
    }

}
