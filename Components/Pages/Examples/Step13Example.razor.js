export function copyById(elementId) {
  try {
    const pre = document.getElementById(elementId);
    if (!pre) return;
    const text = pre.innerText || pre.textContent || "";
    navigator.clipboard.writeText(text);
  } catch (e) {
    console.error("Copy failed", e);
  }
}
