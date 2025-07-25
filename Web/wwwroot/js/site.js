/* Modern Dental Health App JavaScript */

// Global App Object
window.DentalApp = {
    initialized: false,
    
    // Initialize the application
    init: function() {
        if (this.initialized) return;
        
        this.setupEventListeners();
        this.setupMobileMenu();
        this.setupActiveMenu();
        this.setupTooltips();
        this.setupModernEffects();
        
        this.initialized = true;
        console.log('🦷 Dental Health App initialized successfully!');
    },
    
    // Setup global event listeners
    setupEventListeners: function() {
        // Sidebar toggle
        $(document).on('click', '.sidebar-toggle', function() {
            DentalApp.toggleSidebar();
        });
        
        // Close sidebar when clicking outside on mobile
        $(document).on('click', function(e) {
            if (window.innerWidth <= 991 && 
                !$(e.target).closest('.sidebar').length && 
                !$(e.target).hasClass('sidebar-toggle')) {
                $('.sidebar').removeClass('show');
            }
        });
        
        // Smooth scrolling for anchor links
        $(document).on('click', 'a[href^="#"]', function(e) {
            e.preventDefault();
            const target = $(this.getAttribute('href'));
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top - 100
                }, 600);
            }
        });
    },
    
    // Setup mobile menu functionality
    setupMobileMenu: function() {
        $(window).on('resize', function() {
            if (window.innerWidth > 991) {
                $('.sidebar').removeClass('show');
            }
        });
    },
    
    // Setup active menu highlighting
    setupActiveMenu: function() {
        const currentPath = window.location.pathname;
        $('.menu-item').each(function() {
            const href = $(this).attr('href');
            if (href && currentPath.includes(href.split('/')[1])) {
                $('.menu-item').removeClass('active');
                $(this).addClass('active');
            }
        });
    },
    
    // Setup Bootstrap tooltips
    setupTooltips: function() {
        $('[data-bs-toggle="tooltip"]').each(function() {
            new bootstrap.Tooltip(this);
        });
    },
    
    // Setup modern visual effects
    setupModernEffects: function() {
        // Add hover effects to cards
        $('.modern-card, .stats-card').hover(
            function() {
                $(this).addClass('animate__animated animate__pulse');
            },
            function() {
                $(this).removeClass('animate__animated animate__pulse');
            }
        );
        
        // Intersection Observer for animations
        if (window.IntersectionObserver) {
            const observerOptions = {
                threshold: 0.1,
                rootMargin: '0px 0px -50px 0px'
            };
            
            const observer = new IntersectionObserver(function(entries) {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        $(entry.target).addClass('animate__animated animate__fadeInUp');
                    }
                });
            }, observerOptions);
            
            $('.stats-card, .modern-card').each(function() {
                observer.observe(this);
            });
        }
    },
    
    // Toggle sidebar visibility
    toggleSidebar: function() {
        $('.sidebar').toggleClass('show');
    },
    
    // Show modern toast notification
    showToast: function(message, type = 'info', duration = 3000) {
        const iconMap = {
            success: 'fa-check-circle',
            error: 'fa-times-circle',
            warning: 'fa-exclamation-triangle',
            info: 'fa-info-circle'
        };
        
        const toast = $(`
            <div class="toast-notification toast-${type}">
                <i class="fas ${iconMap[type] || iconMap.info}"></i>
                <span>${message}</span>
                <button type="button" class="btn-close btn-sm ms-auto" onclick="$(this).parent().remove()"></button>
            </div>
        `);
        
        $('body').append(toast);
        
        // Animate in
        setTimeout(() => {
            toast.addClass('show');
        }, 100);
        
        // Auto remove
        setTimeout(() => {
            toast.removeClass('show');
            setTimeout(() => toast.remove(), 300);
        }, duration);
        
        return toast;
    },
    
    // Format numbers with Turkish locale
    formatNumber: function(number) {
        return new Intl.NumberFormat('tr-TR').format(number);
    },
    
    // Format date with Turkish locale
    formatDate: function(date, options = {}) {
        const defaultOptions = {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            ...options
        };
        return new Intl.DateTimeFormat('tr-TR', defaultOptions).format(new Date(date));
    },
    
    // Show loading state
    showLoading: function(element) {
        const $element = $(element);
        $element.data('original-html', $element.html());
        $element.html(`
            <div class="d-flex align-items-center justify-content-center">
                <div class="loading-spinner me-2"></div>
                <span>Yükleniyor...</span>
            </div>
        `);
        $element.prop('disabled', true);
    },
    
    // Hide loading state
    hideLoading: function(element) {
        const $element = $(element);
        const originalHtml = $element.data('original-html');
        if (originalHtml) {
            $element.html(originalHtml);
        }
        $element.prop('disabled', false);
    },
    
    // AJAX helper with error handling
    ajax: function(options) {
        const defaultOptions = {
            type: 'GET',
            dataType: 'json',
            beforeSend: function() {
                if (options.loadingElement) {
                    DentalApp.showLoading(options.loadingElement);
                }
            },
            complete: function() {
                if (options.loadingElement) {
                    DentalApp.hideLoading(options.loadingElement);
                }
            },
            error: function(xhr, status, error) {
                console.error('AJAX Error:', error);
                DentalApp.showToast('Bir hata oluştu. Lütfen tekrar deneyin.', 'error');
                if (options.onError) {
                    options.onError(xhr, status, error);
                }
            }
        };
        
        return $.ajax($.extend(defaultOptions, options));
    },
    
    // Chart utilities
    Chart: {
        defaultColors: [
            '#007bff', '#28a745', '#ffc107', '#dc3545', '#17a2b8',
            '#6f42c1', '#fd7e14', '#20c997', '#6c757d', '#343a40'
        ],
        
        getGradient: function(ctx, color1, color2) {
            const gradient = ctx.createLinearGradient(0, 0, 0, 400);
            gradient.addColorStop(0, color1);
            gradient.addColorStop(1, color2);
            return gradient;
        },
        
        defaultOptions: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 20,
                        usePointStyle: true,
                        font: {
                            family: 'Inter',
                            size: 12
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: '#f1f3f4',
                        borderDash: [5, 5]
                    },
                    ticks: {
                        font: {
                            family: 'Inter',
                            size: 11
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    },
                    ticks: {
                        font: {
                            family: 'Inter',
                            size: 11
                        }
                    }
                }
            }
        }
    },
    
    // Utility functions
    Utils: {
        // Debounce function
        debounce: function(func, wait, immediate) {
            let timeout;
            return function executedFunction() {
                const context = this;
                const args = arguments;
                const later = function() {
                    timeout = null;
                    if (!immediate) func.apply(context, args);
                };
                const callNow = immediate && !timeout;
                clearTimeout(timeout);
                timeout = setTimeout(later, wait);
                if (callNow) func.apply(context, args);
            };
        },
        
        // Throttle function
        throttle: function(func, limit) {
            let inThrottle;
            return function() {
                const args = arguments;
                const context = this;
                if (!inThrottle) {
                    func.apply(context, args);
                    inThrottle = true;
                    setTimeout(() => inThrottle = false, limit);
                }
            }
        },
        
        // Generate random ID
        randomId: function() {
            return Math.random().toString(36).substr(2, 9);
        },
        
        // Check if element is in viewport
        isInViewport: function(element) {
            const rect = element.getBoundingClientRect();
            return (
                rect.top >= 0 &&
                rect.left >= 0 &&
                rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
                rect.right <= (window.innerWidth || document.documentElement.clientWidth)
            );
        }
    }
};

// Quick Activity Modal functionality
window.QuickActivity = {
    selectedType: null,
    selectedRating: 5,
    
    init: function() {
        this.setupEventListeners();
        this.setDefaults();
    },
    
    setupEventListeners: function() {
        // Activity type selection
        $(document).on('click', '.activity-type-btn', function() {
            $('.activity-type-btn').removeClass('active');
            $(this).addClass('active');
            QuickActivity.selectedType = $(this).data('type');
            $('#activityType').val(QuickActivity.selectedType);
        });
        
        // Rating stars
        $(document).on('click', '.rating-stars i', function() {
            const rating = $(this).data('rating');
            QuickActivity.selectedRating = rating;
            $('#activityRating').val(rating);
            
            $('.rating-stars i').removeClass('active');
            $('.rating-stars i').each(function() {
                if ($(this).data('rating') <= rating) {
                    $(this).addClass('active');
                }
            });
        });
        
        // Modal reset on hide
        $('#quickActivityModal').on('hidden.bs.modal', function() {
            QuickActivity.reset();
        });
    },
    
    setDefaults: function() {
        // Set first activity type as default
        $('.activity-type-btn').first().addClass('active');
        this.selectedType = $('.activity-type-btn').first().data('type');
        $('#activityType').val(this.selectedType);
        
        // Set 5 stars as default
        $('.rating-stars i').addClass('active');
        this.selectedRating = 5;
        $('#activityRating').val(5);
    },
    
    submit: function() {
        if (!this.validate()) {
            return;
        }
        
        const data = {
            type: parseInt(this.selectedType),
            duration: parseInt($('#activityDuration').val()) * 60, // Convert to seconds
            notes: $('#activityNotes').val().trim()
        };
        
        DentalApp.ajax({
            url: '/Dashboard/QuickActivity',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            loadingElement: '.modal-footer .btn-primary',
            success: function(response) {
                if (response.success) {
                    $('#quickActivityModal').modal('hide');
                    DentalApp.showToast(response.message, 'success');
                    
                    // Refresh page after a short delay
                    setTimeout(() => {
                        window.location.reload();
                    }, 1500);
                } else {
                    DentalApp.showToast(response.message, 'error');
                }
            }
        });
    },
    
    validate: function() {
        if (!this.selectedType) {
            DentalApp.showToast('Lütfen bir aktivite türü seçin', 'warning');
            return false;
        }
        
        const duration = $('#activityDuration').val();
        if (!duration || duration < 1 || duration > 120) {
            DentalApp.showToast('Lütfen 1-120 dakika arasında bir süre girin', 'warning');
            return false;
        }
        
        return true;
    },
    
    reset: function() {
        $('#quickActivityForm')[0].reset();
        $('.activity-type-btn').removeClass('active');
        $('.rating-stars i').removeClass('active');
        this.setDefaults();
    }
};

// Initialize when document is ready
$(document).ready(function() {
    DentalApp.init();
    QuickActivity.init();
});

// Global function for quick activity submission (called from inline onclick)
function submitQuickActivity() {
    QuickActivity.submit();
}

// Global function for sidebar toggle (called from inline onclick)
function toggleSidebar() {
    DentalApp.toggleSidebar();
}

// Service Worker registration for PWA
if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
        navigator.serviceWorker.register('/sw.js')
            .then(function(registration) {
                console.log('🔧 Service Worker registered successfully:', registration.scope);
            })
            .catch(function(error) {
                console.log('❌ Service Worker registration failed:', error);
            });
    });
}

// Add keyboard shortcuts
$(document).keydown(function(e) {
    // Ctrl/Cmd + K for quick activity modal
    if ((e.ctrlKey || e.metaKey) && e.keyCode === 75) {
        e.preventDefault();
        $('#quickActivityModal').modal('show');
    }
    
    // Escape to close sidebar on mobile
    if (e.keyCode === 27 && window.innerWidth <= 991) {
        $('.sidebar').removeClass('show');
    }
});

// Add modern loading overlay
DentalApp.showPageLoading = function() {
    if ($('#pageLoadingOverlay').length === 0) {
        $('body').append(`
            <div id="pageLoadingOverlay" class="page-loading-overlay">
                <div class="loading-content">
                    <div class="loading-spinner"></div>
                    <p>Yükleniyor...</p>
                </div>
            </div>
        `);
    }
    $('#pageLoadingOverlay').fadeIn(300);
};

DentalApp.hidePageLoading = function() {
    $('#pageLoadingOverlay').fadeOut(300, function() {
        $(this).remove();
    });
};

// Add page loading overlay styles
$('<style>').text(`
    .page-loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.9);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
        backdrop-filter: blur(5px);
    }
    
    .loading-content {
        text-align: center;
        color: var(--primary-color);
    }
    
    .loading-content p {
        margin-top: 1rem;
        font-weight: 600;
    }
`).appendTo('head');

console.log('🦷 Dental Health App JavaScript loaded successfully!');
