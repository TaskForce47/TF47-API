<?xml version="1.0" encoding="iso-8859-1"?>
<!-- Transformation XSLT-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- Modèles Implicites source: http://www.grappa.univ-lille3.fr/~jousse/enseignement/XML_XSLT/xslt.html -->
  <xsl:template match="text()">
    <xsl:value-of select="."/>
  </xsl:template>
  <xsl:template match="*">
    <xsl:apply-templates/>
  </xsl:template>
  <xsl:template match="/">

    <!--	Fichier XSL
		Template pour le squad.xml d'ArmA2 réalisé par KissDavid le 11.07.09
		edit for www.taskforce47.de / www.armasim.de by symrex
-->

    <!-- HTML -->
    <html>
      <head>
        <title>
          <xsl:value-of select="/squad/@nick"/>
        </title>
        <link rel="stylesheet" type="text/css" href="/squadxml/squad.css" title="CSS" />
        <link rel="shortcut icon" href="/squadxml/favicon.ico" type="image/x-icon" />
      </head>

      <body>
        <div id="cadre">
          <!-- Menu -->
          <div id="menu">

            <img id="logo" alt="Squad Logo">
              <xsl:attribute name="src">
                <xsl:value-of select="concat(substring-before(/squad/picture,'.paa'),'.jpg')"/>
              </xsl:attribute>
            </img>


            <h3>
              <xsl:value-of select="/squad/name"/>
            </h3>
            <div id="inhalt">
              <p>
                <em>Tag : </em>[<xsl:value-of select="/squad/@nick"/>]
              </p>
              <p>
                <em>Email : </em>
                <xsl:value-of select="/squad/email"/>
              </p>
              <p>
                <em>Web Site : </em>
                <a>
                  <xsl:attribute name="href">
                    <xsl:value-of select="concat('http://',/squad/web)"/>
                  </xsl:attribute>
                  <xsl:value-of select="/squad/web"/>
                </a>
              </p>
              <p>
                <em>Title : </em>
                <xsl:value-of select="/squad/title"/>
              </p>
            </div>
            <!--<pe><em>Member: </em><xsl:value-of select="/squad/count"/></pe> -->
          </div>

          <!-- Liste des membres -->
          <div id="liste">
            <table class="fiche">
              <tr>
                <th>Members</th>
                <th>Remark</th>
                <th>Names</th>
                <th>E-Mail</th>
                <th>Contact</th>
              </tr>
              <xsl:for-each select="/squad/member">
                <tr>
                  <!-- Alternance des couleurs (Nombre pair/ impair)-->
                  <xsl:attribute name="class">
                    <xsl:choose>
                      <xsl:when test="position() mod 2 = 0">impair</xsl:when>
                      <xsl:otherwise>pair</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <td class="nick" >
                    <xsl:value-of select="@nick"/>
                  </td>
                  <td class="remark" >
                    <xsl:value-of select="remark"/>
                  </td>
                  <td class="name" >
                    <xsl:value-of select="name"/>
                  </td>
                  <td class="email">
                    <xsl:value-of select="email"/>
                  </td>
                  <td class="icq">
                    <xsl:value-of select="icq"/>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </div>

          <!-- Liste des membres -->
          <div id="footer">
            <p id="copyright">
              © The ArmA 3 Wallpaper are the property of Bohemia Interactive (http://www.bistudio.com/) - Available in the website ARMA 3<br/>
              Template for the "squad.xml" of ArmA 3 - Realized by KissDavid - 2013
              edit by symrex<br/>
              Buy the Game here: <a href="http://store.bistudio.com" target="_blank" title="Store.bistudio.com">STORE.BISTUDIO.COM</a>
            </p>
          </div>

        </div>
      </body>
    </html>
    <!-- Fin HTML -->

  </xsl:template>

</xsl:stylesheet>