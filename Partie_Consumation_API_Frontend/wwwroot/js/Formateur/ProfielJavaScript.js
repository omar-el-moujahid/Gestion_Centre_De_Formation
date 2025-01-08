// Fonction pour afficher les différentes sections (onglets)
function showTab(tabId) {
    // Cacher tous les onglets
    document.querySelectorAll('.tab-pane').forEach(tab => {
        tab.classList.remove('show', 'active');
    });

    // Afficher l'onglet sélectionné
    const selectedTab = document.getElementById(tabId);
    if (selectedTab) {
        selectedTab.classList.add('show', 'active');
    }

    // Gérer l'état actif des boutons de navigation
    document.querySelectorAll('.action-buttons .btn').forEach(btn => {
        btn.classList.remove('active');
        const btnTabId = btn.getAttribute('onclick').match(/'([^']+)'/)[1];
        if (btnTabId === tabId && tabId !== 'add-formation') {
            btn.classList.add('active');
        }
    });
}

// Gestion dynamique des groupes de médias
document.addEventListener('DOMContentLoaded', function () {
    const addMediaButton = document.getElementById('add-media');
    const mediaContainer = document.getElementById('media-container');

    if (addMediaButton && mediaContainer) {
        addMediaButton.addEventListener('click', function () {
            // Création d'un nouveau groupe de médias
            const mediaGroup = document.createElement('div');
            mediaGroup.className = 'media-group border p-3 mb-3';

            mediaGroup.innerHTML = `
                <div class="mb-3">
                    <label class="form-label">Titre du Média</label>
                    <input type="text" class="form-control" name="MediaTitre[]" required>
                </div>
                <div class="mb-3">
                    <label class="form-label">Type</label>
                    <select class="form-control" name="MediaType[]">
                        <option value="Video">Vidéo</option>
                        <option value="Document">Document</option>
                    </select>
                </div>
                <div class="mb-3">
                    <label class="form-label">URL</label>
                    <input type="text" class="form-control" name="MediaUrl[]" required>
                </div>
                <div class="mb-3">
                    <label class="form-label">Nombre de Séquences</label>
                    <input type="number" class="form-control" name="NombreDeSequence[]" required min="1">
                </div>
                <button type="button" class="btn btn-danger btn-sm remove-media">Supprimer</button>
            `;

            // Ajout du nouveau groupe de médias au conteneur
            mediaContainer.appendChild(mediaGroup);

            // Ajout d'un écouteur pour le bouton de suppression
            mediaGroup.querySelector('.remove-media').addEventListener('click', function () {
                mediaGroup.remove();
            });
        });
    }
});

// Dynamique chargement de contenu dans le modal
document.addEventListener('click', function (event) {
    const target = event.target;
    if (target.matches('[data-bs-toggle="modal"]') && target.dataset.formationId) {
        const formationId = target.dataset.formationId;

        // Appel AJAX pour charger le contenu du modal
        fetch(`/Formation/Edit?id=${formationId}`)
            .then(response => response.text())
            .then(html => {
                const editFormationContent = document.getElementById('editFormationContent');
                if (editFormationContent) {
                    editFormationContent.innerHTML = html;
                }
            })
            .catch(error => {
                console.error('Erreur lors du chargement du contenu du modal:', error);
            });
    }
});


