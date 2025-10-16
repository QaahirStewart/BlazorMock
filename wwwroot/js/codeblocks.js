// Enhances <pre> code blocks by wrapping them in a collapsible <details> and adding a Copy button
export function enhancePreBlocks() {
  try {
    const pres = Array.from(document.querySelectorAll("pre"));
    const createdContainers = [];
    for (const pre of pres) {
      // Skip if already enhanced or already inside a details element
      if (pre.dataset.enhanced === "1" || pre.closest("details")) continue;

      // Create container + header + details/summary
      const container = document.createElement("div");
      container.className = "codeblock-container";

      const header = document.createElement("div");
      header.className =
        "text-[11px] sm:text-xs font-semibold uppercase tracking-wide text-gray-500 mb-2";
      // Support custom titles via data-code-title on the <pre> or its parent
      const customTitle =
        pre.getAttribute("data-code-title") ||
        (pre.parentElement &&
          pre.parentElement.getAttribute &&
          pre.parentElement.getAttribute("data-code-title"));
      header.textContent = customTitle || "Code";

      const details = document.createElement("details");
      details.className = "group";

      const summary = document.createElement("summary");
      summary.className =
        "inline-flex items-center gap-2 px-3 py-1.5 rounded border border-gray-300 bg-white text-xs sm:text-sm cursor-pointer select-none";
      summary.innerHTML =
        'Show code <span class="text-gray-500 group-open:hidden">(click to expand)</span><span class="text-gray-500 hidden group-open:inline">(click to collapse)</span>';

      const wrapper = document.createElement("div");
      wrapper.className = "relative mt-3";

      const copyBtn = document.createElement("button");
      copyBtn.type = "button";
      copyBtn.className =
        "absolute top-2 right-2 px-3 py-1.5 rounded border border-gray-300 bg-white hover:bg-gray-50 text-xs sm:text-sm";
      copyBtn.textContent = "Copy";
      copyBtn.addEventListener("click", () => {
        try {
          const text = pre.innerText || pre.textContent || "";
          navigator.clipboard.writeText(text);
        } catch (e) {
          console.error("Copy failed", e);
        }
      });

      // Insert container before the original <pre>, then move the <pre> into our wrapper
      if (pre.parentElement) {
        pre.parentElement.insertBefore(container, pre);
      }

      wrapper.appendChild(copyBtn);
      wrapper.appendChild(pre); // moves the original node (preserves id/attrs)

      // Build structure: container -> header + details
      details.appendChild(summary);
      details.appendChild(wrapper);
      container.appendChild(header);
      container.appendChild(details);

      // Mark enhanced on the original pre we moved
      pre.dataset.enhanced = "1";
      createdContainers.push(container);
    }

    // Add a simple per-page toolbar to expand/collapse all, once, if opted-in.
    // Opt-in by adding either:
    //   <body data-code-toolbar="1">  OR  an element with [data-code-toolbar='true'] on the page
    // Project decision: Do not render a page-level expand/collapse toolbar.
    // If needed in the future, implement it locally in specific pages rather than globally here.
  } catch (e) {
    console.error("enhancePreBlocks error", e);
  }
}
