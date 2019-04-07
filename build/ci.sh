#!/bin/bash
set -e
__dir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

. "${__dir}/build.sh"
. "${__dir}/test.sh"

bash <(curl -s https://codecov.io/bash) -f tests/opencover.xml