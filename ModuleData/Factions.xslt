<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>

    <xsl:template match="Faction[@id='player_faction']/@banner_key">
    <xsl:attribute name="banner_key">20.163.166.2000.2000.764.764.0.0.0.302.171.171.483.483.764.764.0.0.0</xsl:attribute>
</xsl:template>
</xsl:stylesheet>
