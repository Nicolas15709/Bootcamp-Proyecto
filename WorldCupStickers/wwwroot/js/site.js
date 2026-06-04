/* World Cup Sticker Manager — Interactive JS */

document.addEventListener('DOMContentLoaded', () => {

  // ── 3D Tilt effect on sticker cards ─────────────────────
  function initTilt() {
    document.querySelectorAll('.cromo-card').forEach(card => {
      card.addEventListener('mousemove', e => {
        const r    = card.getBoundingClientRect();
        const x    = e.clientX - r.left;
        const y    = e.clientY - r.top;
        const cx   = r.width  / 2;
        const cy   = r.height / 2;
        const rotX = ((y - cy) / cy) * -9;
        const rotY = ((x - cx) / cx) *  9;
        card.style.transform =
          `perspective(700px) rotateX(${rotX}deg) rotateY(${rotY}deg) translateY(-4px) scale(1.02)`;
      });

      card.addEventListener('mouseleave', () => {
        card.style.transform = '';
        card.style.transition = 'transform .5s cubic-bezier(0.23,1,0.32,1), box-shadow .35s ease';
        setTimeout(() => { card.style.transition = ''; }, 500);
      });

      card.addEventListener('mouseenter', () => {
        card.style.transition = 'transform .12s ease, box-shadow .35s ease';
      });
    });
  }

  initTilt();

  // Re-init after any AJAX updates
  document.addEventListener('cromosUpdated', initTilt);


  // ── Animated stat counters ───────────────────────────────
  function animateCounter(el) {
    const target   = parseInt(el.dataset.target, 10);
    const duration = 1200;
    const step     = 16;
    const steps    = duration / step;
    let   count    = 0;

    const timer = setInterval(() => {
      count += target / steps;
      if (count >= target) {
        el.textContent = target.toLocaleString();
        clearInterval(timer);
      } else {
        el.textContent = Math.floor(count).toLocaleString();
      }
    }, step);
  }

  const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const el = entry.target;
        if (!el.dataset.animated) {
          el.dataset.animated = '1';
          animateCounter(el);
        }
      }
    });
  }, { threshold: 0.3 });

  document.querySelectorAll('.stat-num[data-target]').forEach(el => {
    el.textContent = '0';
    observer.observe(el);
  });


  // ── Fade-up animation via IntersectionObserver ───────────
  const fadeObserver = new IntersectionObserver(entries => {
    entries.forEach(e => {
      if (e.isIntersecting) {
        e.target.style.opacity  = '1';
        e.target.style.transform = 'translateY(0)';
        fadeObserver.unobserve(e.target);
      }
    });
  }, { threshold: 0.1 });

  document.querySelectorAll('.fade-up').forEach(el => {
    el.style.opacity   = '0';
    el.style.transform = 'translateY(22px)';
    el.style.transition = 'opacity .45s ease, transform .45s ease';
    fadeObserver.observe(el);
  });


  // ── Navbar scroll behavior ───────────────────────────────
  const navbar = document.querySelector('.wcs-navbar');
  if (navbar) {
    window.addEventListener('scroll', () => {
      if (window.scrollY > 20) {
        navbar.style.background = 'rgba(4,8,15,.97)';
      } else {
        navbar.style.background = 'rgba(7,13,26,.92)';
      }
    }, { passive: true });
  }


  // ── Dismiss alerts automatically after 5s ───────────────
  document.querySelectorAll('.alert.alert-success').forEach(alert => {
    setTimeout(() => {
      alert.style.transition = 'opacity .5s ease, transform .5s ease';
      alert.style.opacity    = '0';
      alert.style.transform  = 'translateY(-8px)';
      setTimeout(() => alert.remove(), 500);
    }, 5000);
  });

});
