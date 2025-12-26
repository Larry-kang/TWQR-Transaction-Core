---
trigger: model_decision
description: When user asks about Google Apps Script (GAS), Google Sheets automation, Clasp, JavaScript scripting, or lightweight workflow automation.
---

# Project Type: Google Apps Script (GAS) Automation

# Technology Stack
- **Runtime**: Google Apps Script (V8 Engine).
- **Language**: JavaScript (ES6+ features allowed).
- **Deployment**: Google Clasp (Command Line Apps Script Projects).
- **CI/CD**: GitHub Actions (Recommended for large projects, optional for single scripts).

# GAS Specific Rules
1. **Structure**: 
   - Separate Logic (.js/.gs) from View (.html).
   - Use "appsscript.json" for manifest configuration.
2. **Clasp Compatibility**:
   - If using CI/CD, include a step to fix the ".clasprc.json" token format (Node.js script).
   - Always assume the user is on Windows (PowerShell) for local development.
3. **Security**:
   - Never hardcode API Keys or Secrets. Use "PropertiesService.getScriptProperties()".
4. **Reliability**:
   - Implement "Try-Catch" blocks for all external API calls (UrlFetchApp).
   - Use "Utilities.sleep()" responsibly to avoid hitting rate limits.

# Execution Instructions for GAS
1. **Script Generation**: Provide the full ".js" file content.
2. **Manifest**: Always provide the "appsscript.json" content if scopes/permissions are needed.
3. **Deployment**: If complex, provide a "deploy.ps1" script to handle "clasp push".