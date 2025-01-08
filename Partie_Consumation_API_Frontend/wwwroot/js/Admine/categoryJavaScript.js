






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

