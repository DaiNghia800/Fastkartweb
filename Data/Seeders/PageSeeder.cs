using Fastkart.Models.EF;
using Fastkart.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fastkart.Data.Seeders
{
    public class PageSeeder
    {
        private readonly ApplicationDbContext _context;

        public PageSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Đồng bộ các trang thực tế trong project vào database
        /// </summary>
        public void SyncPagesFromViews()
        {
            Console.WriteLine("🔄 Starting Pages synchronization...");

            // Lấy admin user (hoặc user đầu tiên)
            var adminUser = _context.Users.FirstOrDefault(u => u.Deleted == false);

            if (adminUser == null)
            {
                Console.WriteLine("❌ No user found. Please create a user first.");
                return;
            }

            // Danh sách các trang thực tế trong project
            var projectPages = new List<(string Title, string Slug, string Description)>
            {
                ("About Us", "about-us", "Learn more about our company and mission"),
                ("Contact", "contact", "Get in touch with our team"),
                ("Privacy Policy", "privacy-policy", "Our privacy policy and data handling practices"),
                ("Terms & Conditions", "terms-conditions", "Terms of service and user agreements"),
                ("Shipping Policy", "shipping-policy", "Information about shipping methods and delivery"),
                ("Return Policy", "return-policy", "Our return and refund policy"),
                ("FAQ", "faq", "Frequently asked questions and answers"),
                ("Checkout", "checkout", "Complete your purchase"),
                ("Blog", "blog", "Latest news and articles"),
                ("Shop", "shop", "Browse our products")
            };

            int added = 0;
            int updated = 0;
            int skipped = 0;

            foreach (var (title, slug, description) in projectPages)
            {
                try
                {
                    // Kiểm tra xem page đã tồn tại chưa
                    var existingPage = _context.Pages
                        .FirstOrDefault(p => p.Slug == slug && p.Deleted == false);

                    if (existingPage == null)
                    {
                        // Tạo mới page
                        var newPage = new Pages
                        {
                            Title = title,
                            Slug = slug,
                            Content = GenerateDefaultContent(title, description),
                            Status = "Published",
                            AuthorUid = adminUser.Uid,
                            PublishedAt = DateTime.Now,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            CreatedBy = adminUser.FullName,
                            UpdatedBy = adminUser.FullName,
                            Deleted = false
                        };

                        _context.Pages.Add(newPage);
                        added++;
                        Console.WriteLine($"  ✅ Added: {title}");
                    }
                    else
                    {
                        // Cập nhật nếu cần (chỉ cập nhật nếu content rỗng)
                        if (string.IsNullOrWhiteSpace(existingPage.Content))
                        {
                            existingPage.Content = GenerateDefaultContent(title, description);
                            existingPage.UpdatedAt = DateTime.Now;
                            existingPage.UpdatedBy = adminUser.FullName;
                            _context.Entry(existingPage).State = EntityState.Modified;
                            updated++;
                            Console.WriteLine($"  🔄 Updated: {title}");
                        }
                        else
                        {
                            skipped++;
                            Console.WriteLine($"  ⏭️  Skipped: {title} (already exists with content)");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ❌ Error processing {title}: {ex.Message}");
                }
            }

            // Lưu thay đổi
            try
            {
                _context.SaveChanges();
                Console.WriteLine($"\n📊 Summary:");
                Console.WriteLine($"   • Added: {added} pages");
                Console.WriteLine($"   • Updated: {updated} pages");
                Console.WriteLine($"   • Skipped: {skipped} pages");
                Console.WriteLine($"   • Total: {projectPages.Count} pages processed");
                Console.WriteLine("✅ Pages synchronization completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving changes: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo nội dung mặc định cho page
        /// </summary>
        private string GenerateDefaultContent(string title, string description)
        {
            return $@"
<div class='page-content'>
    <h1>{title}</h1>
    <p class='lead'>{description}</p>
    
    <div class='content-section'>
        <h2>Overview</h2>
        <p>This is the {title.ToLower()} page. You can customize this content through the admin panel.</p>
        
        <h3>Key Features</h3>
        <ul>
            <li>Easy to customize</li>
            <li>Responsive design</li>
            <li>SEO optimized</li>
            <li>Fast loading</li>
        </ul>
        
        <h3>Additional Information</h3>
        <p>For more information or questions, please contact our support team at <a href='mailto:support@fastkart.com'>support@fastkart.com</a>.</p>
    </div>
    
    <div class='content-footer'>
        <p><small><em>Last updated: {DateTime.Now:MMMM dd, yyyy}</em></small></p>
    </div>
</div>

<style>
    .page-content {{ padding: 20px; }}
    .page-content h1 {{ color: #009289; margin-bottom: 15px; }}
    .page-content .lead {{ font-size: 1.2rem; color: #666; margin-bottom: 30px; }}
    .content-section {{ margin: 30px 0; }}
    .content-section h2 {{ color: #333; margin-top: 25px; }}
    .content-section h3 {{ color: #555; margin-top: 20px; }}
    .content-section ul {{ margin-left: 20px; }}
    .content-section li {{ margin: 8px 0; }}
    .content-footer {{ margin-top: 40px; padding-top: 20px; border-top: 1px solid #eee; }}
</style>
";
        }

        /// <summary>
        /// Seed pages với nội dung chi tiết (dùng cho lần đầu tiên)
        /// </summary>
        public void SeedDetailedPages()
        {
            if (_context.Pages.Any())
            {
                Console.WriteLine("⚠️ Pages already exist. Use SyncPagesFromViews() instead.");
                return;
            }

            var adminUser = _context.Users.FirstOrDefault(u => u.Deleted == false);

            if (adminUser == null)
            {
                Console.WriteLine("❌ No user found. Please create a user first.");
                return;
            }

            var pages = new List<Pages>
            {
                new Pages
                {
                    Title = "About Us",
                    Slug = "about-us",
                    Content = @"<h1>Welcome to FastKart</h1>
                               <p class='lead'>Your trusted online shopping destination since 2020</p>
                               <h2>Our Mission</h2>
                               <p>To provide customers with the best shopping experience through quality products, fast delivery, and excellent customer service.</p>
                               <h2>Why Choose Us?</h2>
                               <ul>
                                   <li><strong>Wide Selection:</strong> Over 10,000 products across multiple categories</li>
                                   <li><strong>Competitive Prices:</strong> Best prices guaranteed</li>
                                   <li><strong>Fast Delivery:</strong> Same-day delivery available in major cities</li>
                                   <li><strong>Secure Payment:</strong> Multiple secure payment options</li>
                                   <li><strong>24/7 Support:</strong> Our team is always here to help</li>
                               </ul>
                               <h2>Our Story</h2>
                               <p>Founded in 2020, FastKart has grown to become one of the leading e-commerce platforms in Vietnam, serving thousands of satisfied customers every day.</p>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "Contact",
                    Slug = "contact",
                    Content = @"<h1>Contact Us</h1>
                               <p class='lead'>We'd love to hear from you!</p>
                               
                               <div class='contact-info'>
                                   <h2>Our Office</h2>
                                   <p><i class='fa fa-map-marker'></i> <strong>Address:</strong> 123 Shopping Street, District 1, Ho Chi Minh City, Vietnam</p>
                                   <p><i class='fa fa-phone'></i> <strong>Phone:</strong> +84 123 456 789</p>
                                   <p><i class='fa fa-envelope'></i> <strong>Email:</strong> support@fastkart.com</p>
                                   <p><i class='fa fa-clock'></i> <strong>Business Hours:</strong> Mon-Fri: 9:00 AM - 6:00 PM</p>
                               </div>
                               
                               <h2>Customer Support</h2>
                               <p>For immediate assistance, please use our live chat or call our hotline. Our dedicated support team is ready to help with:</p>
                               <ul>
                                   <li>Order tracking and status</li>
                                   <li>Product inquiries</li>
                                   <li>Returns and refunds</li>
                                   <li>Technical support</li>
                                   <li>Account management</li>
                               </ul>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "Privacy Policy",
                    Slug = "privacy-policy",
                    Content = @"<h1>Privacy Policy</h1>
                               <p><em>Last updated: " + DateTime.Now.ToString("MMMM dd, yyyy") + @"</em></p>
                               
                               <h2>1. Information We Collect</h2>
                               <p>We collect information that you provide directly to us, including:</p>
                               <ul>
                                   <li>Name and contact information</li>
                                   <li>Shipping and billing addresses</li>
                                   <li>Payment information</li>
                                   <li>Order history</li>
                                   <li>Communication preferences</li>
                               </ul>
                               
                               <h2>2. How We Use Your Information</h2>
                               <ul>
                                   <li>Process and fulfill your orders</li>
                                   <li>Send order confirmations and updates</li>
                                   <li>Improve our services and customer experience</li>
                                   <li>Send promotional emails (with your consent)</li>
                                   <li>Prevent fraud and ensure security</li>
                               </ul>
                               
                               <h2>3. Data Security</h2>
                               <p>We implement appropriate security measures to protect your personal information from unauthorized access, alteration, disclosure, or destruction.</p>
                               
                               <h2>4. Your Rights</h2>
                               <p>You have the right to access, correct, or delete your personal information. Contact us at privacy@fastkart.com for any privacy-related requests.</p>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "Terms & Conditions",
                    Slug = "terms-conditions",
                    Content = @"<h1>Terms & Conditions</h1>
                               <p><em>Effective Date: " + DateTime.Now.ToString("MMMM dd, yyyy") + @"</em></p>
                               
                               <h2>1. Acceptance of Terms</h2>
                               <p>By accessing and using FastKart, you accept and agree to be bound by these Terms and Conditions.</p>
                               
                               <h2>2. Product Information & Pricing</h2>
                               <ul>
                                   <li>We strive to provide accurate product information</li>
                                   <li>Prices are subject to change without notice</li>
                                   <li>All prices are in Vietnamese Dong (VND) unless stated otherwise</li>
                                   <li>We reserve the right to limit quantities</li>
                               </ul>
                               
                               <h2>3. Orders and Payment</h2>
                               <ul>
                                   <li>All orders are subject to acceptance and availability</li>
                                   <li>Payment must be received before order processing</li>
                                   <li>We accept various payment methods including credit cards, bank transfers, and COD</li>
                               </ul>
                               
                               <h2>4. User Responsibilities</h2>
                               <p>Users must provide accurate information and maintain the confidentiality of their account credentials.</p>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "Shipping Policy",
                    Slug = "shipping-policy",
                    Content = @"<h1>Shipping Policy</h1>
                               
                               <h2>Shipping Methods & Delivery Times</h2>
                               <table class='table table-bordered'>
                                   <thead>
                                       <tr>
                                           <th>Method</th>
                                           <th>Delivery Time</th>
                                           <th>Cost</th>
                                       </tr>
                                   </thead>
                                   <tbody>
                                       <tr>
                                           <td>Standard Shipping</td>
                                           <td>3-5 business days</td>
                                           <td>30,000 VND</td>
                                       </tr>
                                       <tr>
                                           <td>Express Shipping</td>
                                           <td>1-2 business days</td>
                                           <td>60,000 VND</td>
                                       </tr>
                                       <tr>
                                           <td>Same-Day Delivery</td>
                                           <td>Within 24 hours</td>
                                           <td>100,000 VND</td>
                                       </tr>
                                   </tbody>
                               </table>
                               
                               <h2>Free Shipping</h2>
                               <p>Enjoy free standard shipping on orders over <strong>500,000 VND</strong> within Vietnam.</p>
                               
                               <h2>Order Tracking</h2>
                               <p>Once your order ships, you will receive a tracking number via email. You can track your order in real-time through our website.</p>
                               
                               <h2>International Shipping</h2>
                               <p>We currently ship to select Southeast Asian countries. Shipping costs and delivery times vary by destination. Additional customs fees may apply.</p>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "Return Policy",
                    Slug = "return-policy",
                    Content = @"<h1>Return & Refund Policy</h1>
                               
                               <h2>Return Window</h2>
                               <p>You may return most items within <strong>30 days</strong> of delivery for a full refund or exchange.</p>
                               
                               <h2>Return Conditions</h2>
                               <p>To be eligible for a return, items must meet the following conditions:</p>
                               <ul>
                                   <li>✅ Unused and in original condition</li>
                                   <li>✅ Original packaging and tags intact</li>
                                   <li>✅ Original receipt or proof of purchase</li>
                                   <li>✅ No signs of wear or damage</li>
                               </ul>
                               
                               <h2>Non-Returnable Items</h2>
                               <ul>
                                   <li>❌ Perishable goods (food, flowers)</li>
                                   <li>❌ Intimate or sanitary products</li>
                                   <li>❌ Personalized or custom-made items</li>
                                   <li>❌ Digital downloads</li>
                               </ul>
                               
                               <h2>How to Return</h2>
                               <ol>
                                   <li>Contact our customer service at <a href='mailto:returns@fastkart.com'>returns@fastkart.com</a></li>
                                   <li>Receive return authorization and shipping label</li>
                                   <li>Pack item securely with all original materials</li>
                                   <li>Ship the item back to our warehouse</li>
                                   <li>Refund will be processed within 5-7 business days after receipt</li>
                               </ol>
                               
                               <h2>Refund Method</h2>
                               <p>Refunds will be issued to your original payment method. Please allow 7-10 business days for the refund to appear on your statement.</p>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                },

                new Pages
                {
                    Title = "FAQ",
                    Slug = "faq",
                    Content = @"<h1>Frequently Asked Questions</h1>
                               
                               <div class='faq-section'>
                                   <h2>📦 Ordering & Checkout</h2>
                                   
                                   <h3>Q: How do I place an order?</h3>
                                   <p>A: Simply browse our products, add items to your cart, and proceed to checkout. Follow the easy steps to complete your purchase.</p>
                                   
                                   <h3>Q: Can I modify my order after placing it?</h3>
                                   <p>A: Please contact customer service immediately at support@fastkart.com. We'll do our best to accommodate changes if the order hasn't shipped yet.</p>
                                   
                                   <h3>Q: Do I need an account to make a purchase?</h3>
                                   <p>A: No, you can checkout as a guest. However, creating an account allows you to track orders and save your information for faster checkout.</p>
                               </div>
                               
                               <div class='faq-section'>
                                   <h2>💳 Payment</h2>
                                   
                                   <h3>Q: What payment methods do you accept?</h3>
                                   <p>A: We accept credit/debit cards (Visa, MasterCard), bank transfers, e-wallets (Momo, ZaloPay), and Cash on Delivery (COD).</p>
                                   
                                   <h3>Q: Is my payment information secure?</h3>
                                   <p>A: Yes, all transactions are encrypted using SSL technology. We never store your full credit card information.</p>
                               </div>
                               
                               <div class='faq-section'>
                                   <h2>🚚 Shipping & Delivery</h2>
                                   
                                   <h3>Q: How long does shipping take?</h3>
                                   <p>A: Standard shipping takes 3-5 business days, Express shipping takes 1-2 business days, and Same-day delivery is available in select areas.</p>
                                   
                                   <h3>Q: Do you ship internationally?</h3>
                                   <p>A: Yes, we ship to select Southeast Asian countries. Check our Shipping Policy for details and rates.</p>
                                   
                                   <h3>Q: How can I track my order?</h3>
                                   <p>A: You'll receive a tracking number via email once your order ships. You can track it on our website or through the courier's site.</p>
                               </div>
                               
                               <div class='faq-section'>
                                   <h2>↩️ Returns & Refunds</h2>
                                   
                                   <h3>Q: What is your return policy?</h3>
                                   <p>A: Items can be returned within 30 days in unused condition with original packaging. See our Return Policy for complete details.</p>
                                   
                                   <h3>Q: How long does it take to process a refund?</h3>
                                   <p>A: Refunds are processed within 5-7 business days after we receive the returned item. It may take an additional 7-10 days to appear on your statement.</p>
                               </div>",
                    Status = "Published",
                    AuthorUid = adminUser.Uid,
                    PublishedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = adminUser.FullName,
                    UpdatedBy = adminUser.FullName,
                    Deleted = false
                }
            };

            _context.Pages.AddRange(pages);
            _context.SaveChanges();

            Console.WriteLine($"Successfully seeded {pages.Count} detailed pages!");
        }
    }
}