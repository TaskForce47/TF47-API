<?xml version="1.0" encoding="utf-8"?>
<!-- xsl template for "squad.xml" as used in armed assault, see http://community.bistudio.com/wiki/squad.xml -->
<xsl:stylesheet
	version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="text()">
    <xsl:value-of select="."/>
  </xsl:template>

  <xsl:template match="*">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:template match="/">
    <html>
      <head>
        <base href="../../squadxml/" />
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width,initial-scale=1.0" />
        <title>
          <xsl:value-of select="/squad/name"/> Squad-XML
        </title>
        <link rel="stylesheet" type="text/css" href="squad.css"/>
        <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Oswald|Source+Sans+Pro|Material+Icons"/>
        <link rel="shortcut icon" href="favicon.ico" />
      </head>
      <body>
        <header>
          <a>
            <xsl:attribute name="href">
              <xsl:value-of select="/squad/web"/>
            </xsl:attribute>
            <img src="tf47_logo.png" />
            <h3>
              <xsl:value-of select="/squad/name"/>
            </h3>
          </a>
        </header>
        <main class="members">
          <xsl:for-each select="/squad/member">
            <div class="member">
              <span class="member__nick" >
                <xsl:value-of select="@nick"/>
              </span>
              <div class="member__contact">
                <xsl:if test="email != ''">
                  <a>
                    <xsl:attribute name="href">
                      mailto:<xsl:value-of select="email"/>
                    </xsl:attribute>
                    <xsl:attribute name="title">
                      <xsl:value-of select="email"/>
                    </xsl:attribute>

                    <i class="material-icons">mail_outline</i>
                  </a>
                </xsl:if>
                <a target="_blank">
                  <xsl:attribute name="href">
                    https://steamcommunity.com/profiles/<xsl:value-of select="@id"/>
                  </xsl:attribute>
                  <img>
                    <xsl:attribute name="src">steam.svg</xsl:attribute>
                  </img>
                </a>
              </div>
            </div>
          </xsl:for-each>
        </main>
        <footer>
          <address>
            <xsl:if test="/squad/email != ''">
              Contact:
              <a>
                <xsl:attribute name="href">
                  <xsl:value-of select="/squad/email"/>
                </xsl:attribute>
                <xsl:value-of select="/squad/email"/>
              </a>
            </xsl:if>
          </address>
        </footer>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
