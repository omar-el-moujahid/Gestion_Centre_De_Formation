const search = document.querySelector('.input-group-admine input'),
    table_rows_admine = document.querySelectorAll('.tbody-admine tr');

search.addEventListener('input', searchTable);


function searchTable() {
    const search_data = search.value.trim().toLowerCase();

    table_rows_admine.forEach((row, i) => {
        const table_data = row.textContent.toLowerCase();
        row.classList.toggle('hide', table_data.indexOf(search_data) < 0);
        row.style.setProperty('--delay', i / 25 + 's');
    });

    document.querySelectorAll('tbody tr:not(.hide)').forEach((visible_row, i) => {
        visible_row.style.backgroundColor = (i % 2 === 0) ? '#f2f2f2' : 'transparent';
    });
}


const serch_Students = document.querySelector(".input-group-Students input"),
    table_rows_student = document.querySelectorAll(".td-students");

serch_Students.addEventListener('input', searchTable_students);

function searchTable_students() {
    let search_data = serch_Students.value.toLowerCase();

    table_rows_student.forEach((row, i) => {
        // Extraire les données de nom et d'email
        let table_data_name = row.querySelector('div.text-nowrap').textContent.toLowerCase(),
            table_data_email = row.querySelector('span[data-coreui-i18n="Email_students"]').textContent.toLowerCase();

        // Vérifier si les données correspondent à la recherche
        let condition = (table_data_name.indexOf(search_data) < 0) && (table_data_email.indexOf(search_data) < 0);

        // Ajouter ou supprimer la classe 'hide'
        row.closest('tr').classList.toggle('hide', condition);

        // Ajouter un délai pour l'animation
        row.closest('tr').style.setProperty('--delay', i / 25 + 's');
    });

    // Appliquer des couleurs alternées aux lignes visibles
    document.querySelectorAll('tbody tr:not(.hide)').forEach((visible_row, i) => {
        visible_row.style.backgroundColor = (i % 2 == 0) ? 'transparent' : '#f2f2f2';
    });
}






document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.querySelector('.input-group-Teachers input[type="search"]'); // Champ de recherche
    const tableRows = document.querySelectorAll('.tbody-Teachers tr'); // Les lignes de la table

    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase(); // Convertir la valeur à minuscule pour une recherche insensible à la casse

        tableRows.forEach(row => {
            const cells = row.querySelectorAll('td'); // Les cellules de chaque ligne
            const rowText = Array.from(cells).map(cell => cell.textContent.toLowerCase()).join(' '); // Concaténer le texte des cellules

            // Vérifier si le texte de la ligne contient la valeur recherchée
            if (rowText.includes(searchValue)) {
                row.style.display = ''; // Afficher la ligne
            } else {
                row.style.display = 'none'; // Masquer la ligne
            }
        });
    });
});



document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.querySelector('.input-group-Categories input[type="search"]'); // Champ de recherche
    const tableRows = document.querySelectorAll('.tbody-categories tr'); // Les lignes de la table

    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase(); // Convertir la valeur à minuscule pour une recherche insensible à la casse

        tableRows.forEach(row => {
            const cells = row.querySelectorAll('td'); // Les cellules de chaque ligne
            const rowText = Array.from(cells).map(cell => cell.textContent.toLowerCase()).join(' '); // Concaténer le texte des cellules

            // Vérifier si le texte de la ligne contient la valeur recherchée
            if (rowText.includes(searchValue)) {
                row.style.display = ''; // Afficher la ligne
            } else {
                row.style.display = 'none'; // Masquer la ligne
            }
        });
    });
});




document.addEventListener('DOMContentLoaded', () => {
    const searchInput = document.querySelector('.input-group-Courses input[type="search"]'); // Champ de recherche
    const tableRows = document.querySelectorAll('.tbody-Courses tr'); // Les lignes de la table

    searchInput.addEventListener('input', () => {
        const searchValue = searchInput.value.toLowerCase(); // Convertir la valeur à minuscule pour une recherche insensible à la casse

        tableRows.forEach(row => {
            const cells = row.querySelectorAll('td'); // Les cellules de chaque ligne
            const rowText = Array.from(cells).map(cell => cell.textContent.toLowerCase()).join(' '); // Concaténer le texte des cellules

            // Vérifier si le texte de la ligne contient la valeur recherchée
            if (rowText.includes(searchValue)) {
                row.style.display = ''; // Afficher la ligne
            } else {
                row.style.display = 'none'; // Masquer la ligne
            }
        });
    });
});