<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output omit-xml-declaration="yes"/>
    <xsl:template match="@*|node()">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()"/>
        </xsl:copy>
    </xsl:template>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_ase')]"/>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_bat')]"/>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_emp')]"/>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_khu')]"/>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_stu')]"/>
    <xsl:template match="*[starts-with(@id, 'player_char_creation_vla')]"/>
</xsl:stylesheet>
