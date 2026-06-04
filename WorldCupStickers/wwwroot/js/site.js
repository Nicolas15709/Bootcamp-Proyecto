/* ============================================================
   World Cup Sticker Manager — Interactive JS
   GSAP 3 + VanillaTilt + Holographic + Magnetic
   ============================================================ */

document.addEventListener('DOMContentLoaded', () => {

  // ── Page-load progress bar ─────────────────────────────
  const bar = document.getElementById('page-bar');
  if (bar) {
    let w = 0;
    const t = setInterval(() => {
      w = Math.min(w + Math.random() * 18, 90);
      bar.style.width = w + '%';
    }, 80);
    window.addEventListener('load', () => {
      clearInterval(t);
      bar.style.width = '100%';
      setTimeout(() => { bar.style.opacity = '0'; bar.style.transition = 'opacity .4s'; }, 300);
    });
  }

  // ── GSAP ──────────────────────────────────────────────
  if (typeof gsap !== 'undefined') {

    gsap.registerPlugin(ScrollTrigger);

    // ── Hero entrance — only translateY, no opacity ────
    const hero = document.querySelector('.wcs-hero');
    if (hero) {
      const tl = gsap.timeline({ defaults: { ease: 'power3.out' } });
      tl.from('.hero-eyebrow', { y: 16, duration: .55 })
        .from('.hero-title',   { y: 36, duration: .75 }, '-=.3')
        .from('.hero-sub',     { y: 16, duration: .55 }, '-=.4')
        .from('.hero-btns',    { y: 14, duration: .45 }, '-=.3');
    }

    // ── Stat cards — slide in on scroll ────────────────
    const statCards = gsap.utils.toArray('.stat-card');
    if (statCards.length) {
      gsap.from(statCards, {
        scrollTrigger: { trigger: statCards[0], start: 'top 90%' },
        y: 40, duration: .6, stagger: .09, ease: 'power2.out'
      });
    }

    // ── Cromo cards — scale + slide on scroll ──────────
    const cromoCatalog = document.querySelector('.cromos-grid');
    if (cromoCatalog) {
      gsap.from('.cromos-grid .cromo-card', {
        scrollTrigger: { trigger: cromoCatalog, start: 'top 92%' },
        y: 50, scale: .94,
        duration: .6, stagger: .05, ease: 'power2.out'
      });
    }

    // ── Team cards ─────────────────────────────────────
    gsap.from('.equipo-card', {
      scrollTrigger: { trigger: '.equipo-card', start: 'top 90%' },
      y: 35, duration: .55, stagger: .09, ease: 'power2.out'
    });

    // ── Page header ────────────────────────────────────
    gsap.from('.wcs-page-header', { y: 18, duration: .5, ease: 'power2.out' });

    // ── Filter bar ─────────────────────────────────────
    gsap.from('.filter-bar', { y: 18, duration: .45, delay: .12, ease: 'power2.out' });

    // ── Animated number counters ───────────────────────
    document.querySelectorAll('.stat-num[data-target]').forEach(el => {
      const target = parseInt(el.dataset.target, 10);
      ScrollTrigger.create({
        trigger: el,
        start: 'top 90%',
        once: true,
        onEnter: () => {
          gsap.from({ val: 0 }, {
            val: target,
            duration: 1.4,
            ease: 'power2.out',
            onUpdate() { el.textContent = Math.round(this.targets()[0].val).toLocaleString(); }
          });
        }
      });
    });

  } // end if(gsap)

  // ── VanillaTilt (3D cards) ─────────────────────────
  if (typeof VanillaTilt !== 'undefined') {
    VanillaTilt.init(document.querySelectorAll('.cromo-card'), {
      max: 12, speed: 320, perspective: 650,
      glare: false, scale: 1.04,
      gyroscope: true
    });
  }

  // ── Holographic + Glare mouse tracking ────────────────
  document.querySelectorAll('.cromo-card').forEach(card => {
    card.addEventListener('mousemove', e => {
      const r = card.getBoundingClientRect();
      const x = ((e.clientX - r.left) / r.width)  * 100;
      const y = ((e.clientY - r.top)  / r.height) * 100;
      card.style.setProperty('--mx', x + '%');
      card.style.setProperty('--my', y + '%');
    });

    card.addEventListener('mouseleave', () => {
      card.style.setProperty('--mx', '50%');
      card.style.setProperty('--my', '50%');
    });
  });

  // ── Magnetic buttons ──────────────────────────────────
  document.querySelectorAll('.btn-wcs, .btn-outline-wcs').forEach(btn => {
    btn.addEventListener('mouseenter', () => {
      btn.style.transition = 'transform .15s ease, box-shadow .3s ease, background-position .4s ease';
    });

    btn.addEventListener('mousemove', e => {
      const r  = btn.getBoundingClientRect();
      const dx = (e.clientX - r.left - r.width  / 2) * 0.28;
      const dy = (e.clientY - r.top  - r.height / 2) * 0.28;
      btn.style.transform = `translate(${dx}px, ${dy}px)`;
    });

    btn.addEventListener('mouseleave', () => {
      btn.style.transition = 'transform .55s cubic-bezier(0.23,1,0.32,1), box-shadow .3s ease, background-position .4s ease';
      btn.style.transform  = '';
    });
  });

  // ── Navbar scroll tint ────────────────────────────────
  const navbar = document.querySelector('.wcs-navbar');
  if (navbar) {
    window.addEventListener('scroll', () => {
      navbar.style.background = window.scrollY > 30
        ? 'rgba(3,6,13,.97)'
        : 'rgba(4,8,15,.85)';
    }, { passive: true });
  }

  // ── Auto-dismiss success alerts ───────────────────────
  document.querySelectorAll('.alert-success').forEach(el => {
    setTimeout(() => {
      if (typeof gsap !== 'undefined') {
        gsap.to(el, { opacity: 0, y: -10, duration: .5, onComplete: () => el.remove() });
      } else {
        el.remove();
      }
    }, 5000);
  });

  // ── Bulk update loading overlay ───────────────────────
  document.querySelectorAll('form[action*="ActualizarFotosBulk"]').forEach(form => {
    form.addEventListener('submit', e => {
      if (!confirm('¿Actualizar fotos desde TheSportsDB? Puede tardar unos segundos.')) {
        e.preventDefault(); return;
      }
      const ov = document.createElement('div');
      ov.className = 'loading-overlay';
      ov.innerHTML = `
        <div class="loading-spinner"></div>
        <div class="lo-title">Buscando fotos en TheSportsDB...</div>
        <div style="color:var(--text-2);font-size:.82rem">Esto puede tardar hasta 15 segundos</div>`;
      document.body.appendChild(ov);
      if (typeof gsap !== 'undefined') gsap.from(ov, { opacity: 0, duration: .3 });
    });
  });

  // Re-init VanillaTilt when cards are dynamically added
  document.addEventListener('cromosUpdated', () => {
    if (typeof VanillaTilt !== 'undefined') {
      VanillaTilt.init(document.querySelectorAll('.cromo-card:not([data-tilt])'), {
        max: 12, speed: 320, perspective: 650, glare: false, scale: 1.04
      });
    }
  });

});
