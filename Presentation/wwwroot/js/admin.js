/* ============================================
   ECOMMERCE NA — Admin Panel JavaScript
   Core utilities: sidebar toggle, toasts,
   modals, API helpers, confirmations
   ============================================ */

// ── Sidebar Toggle ──
function toggleSidebar() {
    const sidebar = document.getElementById('adminSidebar');
    const overlay = document.getElementById('sidebarOverlay');
    sidebar.classList.toggle('is-open');
    overlay.classList.toggle('is-visible');
}

function focusGlobalSearch() {
    const search = document.querySelector('.search-input');
    if (search) search.focus();
}

// ── Toast Notifications ──
function showToast(message, type = 'info', duration = 4000) {
    const container = document.getElementById('toastContainer');
    const icons = {
        success: 'check-circle-2',
        error: 'x-circle',
        warning: 'alert-triangle',
        info: 'info'
    };

    const toast = document.createElement('div');
    toast.className = `toast toast--${type}`;
    toast.innerHTML = `
        <span class="toast-icon"><i data-lucide="${icons[type] || 'info'}"></i></span>
        <span class="toast-message">${message}</span>
        <button class="toast-close" onclick="removeToast(this.parentElement)">
            <i data-lucide="x"></i>
        </button>
    `;

    container.appendChild(toast);
    lucide.createIcons({ nodes: [toast] });

    setTimeout(() => removeToast(toast), duration);
}

function removeToast(toast) {
    if (!toast || toast.classList.contains('is-leaving')) return;
    toast.classList.add('is-leaving');
    setTimeout(() => toast.remove(), 300);
}

// ── Modal Helpers ──
function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('is-open');
        document.body.style.overflow = 'hidden';
        // Focus first input
        setTimeout(() => {
            const firstInput = modal.querySelector('input, select, textarea');
            if (firstInput) firstInput.focus();
        }, 200);
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('is-open');
        document.body.style.overflow = '';
    }
}

// Close modal on overlay click
document.addEventListener('click', (e) => {
    if (e.target.classList.contains('modal-overlay') && e.target.classList.contains('is-open')) {
        e.target.classList.remove('is-open');
        document.body.style.overflow = '';
    }
});

// Close modal on Escape
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape') {
        const openModals = document.querySelectorAll('.modal-overlay.is-open');
        openModals.forEach(m => {
            m.classList.remove('is-open');
            document.body.style.overflow = '';
        });
    }
});

// ── API Helpers ──
const API_BASE = '/admin';

async function apiGet(endpoint) {
    const response = await fetch(`${API_BASE}/${endpoint}`);
    if (!response.ok) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `Error ${response.status}`);
    }
    return response.json();
}

async function apiPost(endpoint, data) {
    const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!response.ok) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `Error ${response.status}`);
    }
    return response.json();
}

async function apiPut(endpoint, data) {
    const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!response.ok) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `Error ${response.status}`);
    }
    return response.json();
}

async function apiPatch(endpoint, data) {
    const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    });
    if (!response.ok) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `Error ${response.status}`);
    }
    return response.json();
}

async function apiDelete(endpoint) {
    const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: 'DELETE'
    });
    if (!response.ok && response.status !== 204) {
        const err = await response.json().catch(() => ({}));
        throw new Error(err.message || `Error ${response.status}`);
    }
    return response.status === 204 ? null : response.json();
}

// ── Confirmation Dialog ──
function showConfirm(title, message, onConfirm, options = {}) {
    const { confirmText = 'Confirmar', cancelText = 'Cancelar', danger = false } = options;

    // Remove existing confirm if any
    const existing = document.getElementById('confirmModal');
    if (existing) existing.remove();

    const modal = document.createElement('div');
    modal.className = 'modal-overlay is-open';
    modal.id = 'confirmModal';
    modal.innerHTML = `
        <div class="modal-container" style="max-width:420px;">
            <div class="modal-header">
                <h3 class="modal-title">${title}</h3>
                <button class="modal-close" onclick="closeConfirm()"><i data-lucide="x"></i></button>
            </div>
            <div class="modal-body">
                <div class="confirm-icon ${danger ? 'confirm-icon--danger' : ''}">
                    <i data-lucide="${danger ? 'triangle-alert' : 'help-circle'}"></i>
                </div>
                <p class="confirm-text">${message}</p>
            </div>
            <div class="modal-footer">
                <button class="btn-admin btn-secondary" onclick="closeConfirm()">${cancelText}</button>
                <button class="btn-admin ${danger ? 'btn-danger' : 'btn-primary'}" id="confirmAction">${confirmText}</button>
            </div>
        </div>
    `;

    document.body.appendChild(modal);
    document.body.style.overflow = 'hidden';
    lucide.createIcons({ nodes: [modal] });

    document.getElementById('confirmAction').addEventListener('click', () => {
        closeConfirm();
        onConfirm();
    });
}

function closeConfirm() {
    const modal = document.getElementById('confirmModal');
    if (modal) {
        modal.classList.remove('is-open');
        document.body.style.overflow = '';
        setTimeout(() => modal.remove(), 300);
    }
}

// ── Formatting Helpers ──
function formatCurrency(amount) {
    return new Intl.NumberFormat('es-NI', {
        style: 'currency',
        currency: 'NIO',
        minimumFractionDigits: 2
    }).format(amount);
}

function formatDate(dateStr) {
    const date = new Date(dateStr);
    return new Intl.DateTimeFormat('es-NI', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(date);
}

function formatDateShort(dateStr) {
    const date = new Date(dateStr);
    return new Intl.DateTimeFormat('es-NI', {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
    }).format(date);
}

// ── Skeleton loading placeholders ──
function showTableSkeleton(tableBodyId, cols = 4, rows = 5) {
    const tbody = document.getElementById(tableBodyId);
    if (!tbody) return;
    tbody.innerHTML = '';
    for (let r = 0; r < rows; r++) {
        let cells = '';
        for (let c = 0; c < cols; c++) {
            const widthClass = c === 0 ? 'skeleton-line--long' : (c === cols - 1 ? 'skeleton-line--short' : 'skeleton-line--medium');
            cells += `<td><div class="skeleton skeleton-line ${widthClass}" style="height:14px;"></div></td>`;
        }
        tbody.innerHTML += `<tr>${cells}</tr>`;
    }
}

function showEmptyState(containerId, icon, title, desc, actionBtn = '') {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = `
        <tr>
            <td colspan="100%">
                <div class="empty-state">
                    <div class="empty-state-icon"><i data-lucide="${icon}"></i></div>
                    <div class="empty-state-title">${title}</div>
                    <div class="empty-state-desc">${desc}</div>
                    ${actionBtn}
                </div>
            </td>
        </tr>
    `;
    lucide.createIcons({ nodes: [container] });
}

// ── Debounce ──
function debounce(fn, delay = 300) {
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => fn(...args), delay);
    };
}
