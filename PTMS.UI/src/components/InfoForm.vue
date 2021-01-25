<template>
  <div class="card">
    <div class="card-content">
      <div class="media">
        <div class="media-left">
          <figure class="image is-48x48">
            <img src="/sandro.png" alt="Placeholder image">
          </figure>
        </div>
        <div class="media-content">
          <p class="title is-4">Sandro <b-icon icon="gitlab"></b-icon>
            <a target="_blank" style="margin: 0 5px 0 5px" href="https://gitlab.com/svireli/sandro-front">front-end</a>
            <a target="_blank" style="margin: 0 5px 0 5px" href="https://gitlab.com/svireli/sandro">back-end</a>
          </p>
          <p class="subtitle is-6">By <a href="https://gitlab.com/svireli">Goga Lominashvili</a> (g.lominashvili@gmail.com)</p>
        </div>
      </div>

      <div class="content">
        Sandro is a very performant and resource friendly template management system,
        it uses full chromium engine,
        memory cached precompiled moustache html templates and couchdb
        to render templates blazing fast.
        It also supports many optimization options for input based file caching,
        external resource load optimization and
        header/footer templates.
        Sandro is released under original 4-clause "BSD license",
        contributors are more than welcome.
        <pre class="small">
            Copyright (c) 2018, Goga Lominashvili
            All rights reserved.

            Redistribution and use in source and binary forms, with or without
            modification, are permitted provided that the following conditions are met:
            1. Redistributions of source code must retain the above copyright
               notice, this list of conditions and the following disclaimer.
            2. Redistributions in binary form must reproduce the above copyright
               notice, this list of conditions and the following disclaimer in the
               documentation and/or other materials provided with the distribution.
            3. All advertising materials mentioning features or use of this software
               must display the following acknowledgement:
               This product includes software developed by Goga Lominashvili.
            4. Neither the name of Goga Lominashvili nor the
               names of its contributors may be used to endorse or promote products
               derived from this software without specific prior written permission.

            THIS SOFTWARE IS PROVIDED BY GOGA LOMINASHVILI ''AS IS'' AND ANY
            EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
            WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
            DISCLAIMED. IN NO EVENT SHALL GOGA LOMINASHVILI BE LIABLE FOR ANY
            DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
            (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
            LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
            ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
            (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
            SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
        </pre>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'InfoForm',
};
</script>

<style scoped>
pre.small {
  margin-top: 20px;
  height: 200px;
}
</style>
