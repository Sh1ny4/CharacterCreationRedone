<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output omit-xml-declaration="yes"/>
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>
    <xsl:template match="*[starts-with(@id, 'mother_char_creation_')]"/>
    <xsl:template match="*[starts-with(@id, 'father_char_creation_')]"/>
</xsl:stylesheet>
