# 버전 관리 전략

## 브랜칭 전략
- Trunk Based Development (TBD) 채택
- `main` 브랜치를 유일한 영구 브랜치로 사용
- 기능 개발 시 `feature/` 접두사로 단기 생존 브랜치 생성 (예: `feature/ai-integration`)
- PR 머지 시 Squash Merge 적용

## 커밋 규칙
- Conventional Commits 준수 (feat, fix, docs, style, refactor, test, chore)
- 커밋 메시지 형식: `<type>(<scope>): <subject>`
- 예: `feat(canvas): add brush size control`

## 릴리스 절차
- 시맨틱 버저닝(`v<major>.<minor>.<patch>`) 사용
- GitHub Releases로 배포 패키지 관리
- 태그를 사용한 버전 관리: `git tag v1.0.0`

## CI/CD 파이프라인
- GitHub Actions를 사용한 빌드/테스트 자동화
- 주요 브랜치(main)에 푸시 또는 PR 머지 시 자동 실행
- 빌드 성공 시 어셈블리 버전 자동 증가 (선택적)