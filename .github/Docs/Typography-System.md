# Typography System - Consistency & Responsiveness Guide

## Overview

This document outlines the consistent, responsive typography system implemented across the entire BlazorMock application.

## Global Typography Configuration

### Location: `Styles/input.css`

The typography system uses Tailwind CSS v4 theming with:

- **Font Family**: System font stack for optimal performance and native feel
- **Responsive Font Sizes**: xs, sm, base, lg, xl, 2xl, 3xl, 4xl, 5xl
- **Line Heights**: tight, snug, normal, relaxed, loose
- **Global Text Wrapping**: Ensures all text breaks properly on small screens

### Key Features

1. **Responsive Heading Scales**

   - H1: `text-3xl sm:text-4xl md:text-5xl`
   - H2: `text-xl sm:text-2xl`
   - H3: `text-base sm:text-lg`

2. **Body Text Scales**

   - Large text: `text-base sm:text-lg md:text-xl`
   - Normal text: `text-sm sm:text-base`
   - Small text: `text-xs sm:text-sm`

3. **Text Wrapping**
   - All text uses `break-words` to prevent overflow
   - Code blocks use `break-all` for long strings
   - Buttons and badges use `whitespace-nowrap` to prevent awkward breaks

## Component-by-Component Implementation

### Home.razor

- ✅ Hero heading: Responsive 3xl → 4xl → 5xl
- ✅ Hero text: Responsive base → lg → xl
- ✅ Feature cards: Responsive padding and text sizes
- ✅ Grid: Responsive sm:grid-cols-2 md:grid-cols-3

### Guide.razor

- ✅ Page title: Responsive with break-words
- ✅ Section headings: Consistent xl → 2xl scale
- ✅ Date/time cards: Responsive text and layout
- ✅ Step cards: Flexible layout with proper text wrapping
- ✅ Demo counter: Responsive button sizes

### Progress.razor

- ✅ Page title: Responsive 3xl → 4xl → 5xl
- ✅ Progress card: Flexible layout for mobile
- ✅ Step items: Stack vertically on mobile, horizontal on desktop
- ✅ Buttons: Proper text sizing with whitespace-nowrap

### Counter.razor

- ✅ Heading: Responsive xl → 2xl
- ✅ Counter display: Responsive 3xl → 4xl
- ✅ Buttons: Consistent sizing with responsive padding

### DriverForm.razor

- ✅ Form labels: xs → sm font size
- ✅ Input fields: Responsive text sizing
- ✅ Validation messages: Consistent xs text
- ✅ Responsive grid: 1 column mobile, 2 columns desktop

### Tips.razor

- ✅ Category buttons: Flexible layout with min-w-0
- ✅ Tip cards: Responsive text in titles and content
- ✅ Navigation: Proper spacing and text sizing

### MainLayout.razor

- ✅ Brand logo: Responsive sizing with shrink-0
- ✅ Navigation links: Consistent sizing across breakpoints
- ✅ Mobile menu: Responsive text with sm:text-base

### GreetingCard.razor

- ✅ Component title: Responsive lg → xl
- ✅ Message text: Responsive sm → base
- ✅ Icon sizes: Consistent with shrink-0

### Step1Example.razor (and other examples)

- ✅ Page headers: Responsive 3xl → 4xl
- ✅ Section headings: xl → 2xl scale
- ✅ Code blocks: Responsive xs → sm with proper wrapping
- ✅ Info boxes: Responsive padding and text

## Typography Scale Reference

```css
/* Font Sizes */
text-xs:    12px (0.75rem)
text-sm:    14px (0.875rem)
text-base:  16px (1rem)
text-lg:    18px (1.125rem)
text-xl:    20px (1.25rem)
text-2xl:   24px (1.5rem)
text-3xl:   30px (1.875rem)
text-4xl:   36px (2.25rem)
text-5xl:   48px (3rem)

/* Line Heights */
leading-tight:    1.25
leading-snug:     1.375
leading-normal:   1.5
leading-relaxed:  1.625
leading-loose:    2
```

## Responsive Breakpoints

```css
/* Tailwind Breakpoints */
sm:  640px  (Small tablets and up)
md:  768px  (Tablets and up)
lg:  1024px (Laptops and up)
xl:  1280px (Desktops and up)
```

## Best Practices

### DO ✅

- Use responsive text classes: `text-base sm:text-lg md:text-xl`
- Add `break-words` to all text content
- Use `shrink-0` on icons and badges
- Use `min-w-0` on flex children that should shrink
- Apply `whitespace-nowrap` to buttons and badges
- Use `leading-relaxed` for body text readability

### DON'T ❌

- Use fixed font sizes without responsive variants
- Forget `break-words` on long text content
- Allow text to overflow containers
- Use inconsistent heading sizes across pages
- Neglect mobile testing

## Mobile-First Approach

All styles follow mobile-first design:

1. Base styles target mobile (320px+)
2. `sm:` modifier applies at 640px+
3. `md:` modifier applies at 768px+
4. `lg:` modifier applies at 1024px+

## Testing Checklist

When adding new components:

- [ ] Test at 320px width (small mobile)
- [ ] Test at 640px width (large mobile)
- [ ] Test at 768px width (tablet)
- [ ] Test at 1024px+ width (desktop)
- [ ] Verify no text overflow
- [ ] Check text readability at all sizes
- [ ] Ensure proper line-height for body text

## Maintenance

To update typography globally:

1. Edit `Styles/input.css` theme variables
2. Run: `npx @tailwindcss/cli -i ./Styles/input.css -o ./wwwroot/tailwind.css`
3. Test across all pages
4. Update this documentation

## Responsive CRUD Pattern

All CRUD list pages (Drivers, Trucks, Routes) implement a consistent responsive pattern:

### Mobile Layout (< 768px)

Cards displayed using `md:hidden`:

```html
<div class="space-y-3 md:hidden">
  @foreach (var item in items) {
  <div class="rounded-2xl border border-gray-200 bg-white p-4">
    <!-- Item details -->
    <div class="mb-3">
      <div class="text-base font-semibold text-gray-900">@item.Name</div>
      <div class="text-sm text-gray-600">Detail 1</div>
      <div class="text-sm text-gray-600">Detail 2</div>
    </div>
    <!-- Action buttons -->
    <div class="flex flex-wrap gap-2">
      <button class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm">
        Edit
      </button>
      <button class="px-3 py-2 rounded-lg border hover:bg-gray-50 text-sm">
        Delete
      </button>
    </div>
  </div>
  }
</div>
```

### Desktop Layout (≥ 768px)

Tables displayed using `hidden md:block`:

```html
<div class="overflow-x-auto hidden md:block">
  <table class="min-w-full border border-gray-200 rounded-xl overflow-hidden">
    <thead class="bg-gray-50">
      <tr>
        <th class="text-left p-2 border-b">Column 1</th>
        <th class="text-left p-2 border-b">Column 2</th>
        <th class="text-left p-2 border-b">Actions</th>
      </tr>
    </thead>
    <tbody>
      @foreach (var item in items) {
      <tr class="hover:bg-gray-50">
        <td class="p-2 border-b">@item.Name</td>
        <td class="p-2 border-b">@item.Detail</td>
        <td class="p-2 border-b">
          <div class="flex gap-2">
            <button>Edit</button>
            <button>Delete</button>
          </div>
        </td>
      </tr>
      }
    </tbody>
  </table>
</div>
```

### Breakpoint Strategy

- **md:hidden**: Show on mobile/tablet (< 768px), hide on desktop
- **hidden md:block**: Hide on mobile/tablet, show on desktop (≥ 768px)
- **Benefits**: Native mobile UX with cards, efficient desktop UX with tables

### Edit Mode

Both layouts support inline editing:

- **Mobile**: Labeled inputs in card layout
- **Desktop**: Inline inputs in table cells

This ensures feature parity across all device sizes.

## Summary

The typography system ensures:

- ✅ **Consistent font sizing** across all pages
- ✅ **Responsive text** that scales appropriately
- ✅ **Proper text wrapping** to prevent overflow
- ✅ **Readable line heights** for better UX
- ✅ **Mobile-first design** that works on all devices
- ✅ **Maintainable system** with centralized configuration
- ✅ **Responsive CRUD pattern** with mobile cards and desktop tables

Last Updated: October 16, 2025
