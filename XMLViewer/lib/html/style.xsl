<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
    <xsl:output method='html'/>

    <xsl:template match="NewspaperData">
        <HTML>
            <BODY>
                <DIV style="margin: 50px auto; width: 600px; border: solid 2px gray; padding: 20px">
                    <H1 style="margin: 0;">Online Newspaper of the University</H1>

                    <xsl:apply-templates select="Article"/>
                </DIV>
            </BODY>
        </HTML>
    </xsl:template>

    <xsl:template match="Article">
        <div style="margin: 20px 0px; border-left: solid 4px orange; padding: 0 0 0 10px;">
            <h2 style="margin:0;"><b><xsl:value-of select="Title"/></b></h2>
            
            <p style="margin:10px 0 0 0;"><xsl:value-of select="Annotation"/></p>
            <p><span style="color: gray;">Date: </span><xsl:value-of select="Date"/></p>
            <p><span style="color: gray;">Category: </span> <xsl:value-of select="Category"/></p>
            <p><span style="color: gray;">By </span> <xsl:value-of select="Author"/></p>
            
            <xsl:apply-templates select="Reviews"/>
        </div>
    </xsl:template>

    
    <xsl:template match="Reviews">
        <xsl:for-each select="Review">
            <p>
                <b style="font-size: 22px; margin:0 5px 0 0; color: gray">"</b>
                <xsl:value-of select="self::Review"/>
            </p>
        </xsl:for-each>
    </xsl:template>

</xsl:stylesheet>
