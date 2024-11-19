namespace E_CommerceWebApp.Services
{
    public static class AddLoginProviderServices
    {
        public static void AddGoogleLoginProvider(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthsection = builder.Configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuthsection["ClientId"];
                options.ClientSecret = googleAuthsection["ClientSecret"];


            });
        }
    }
}
