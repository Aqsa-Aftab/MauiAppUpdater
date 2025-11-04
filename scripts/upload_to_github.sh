#!/usr/bin/env bash
set -e
USER="$1"; REPO="$2"; VIS="${3:-public}"
if [ -z "$USER" ] || [ -z "$REPO" ]; then echo "Usage: $0 <github-username> <repo-name> [visibility]"; exit 2; fi
git init; git add -A; git commit -m "Initial commit"; git branch -M main
if command -v gh >/dev/null 2>&1; then gh repo create "$USER/$REPO" --"$VIS" --source=. --remote=origin --push || true; else echo "Create repo manually and add remote"; fi
git checkout -b release/v1.0.0; git push -u origin release/v1.0.0 || true
if command -v gh >/dev/null 2>&1; then gh pr create --title "Prepare release v1.0.0" --body-file .github/PR_BODY.md --base main --head release/v1.0.0 || true; fi
echo "Done."
