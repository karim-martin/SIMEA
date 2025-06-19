-- Enterprise Architecture Demo Data for Banking
-- Creates 4 records for each artifact type in all 4 domains
-- Includes complex properties and artifact linkages

-- ==========================================
-- BUSINESS ARCHITECTURE DOMAIN - 6 Artifact Types
-- ==========================================

-- 1. BusinessStrategyView (4 records)
INSERT INTO BusinessStrategyViews (BusinessGoal, StrategicObjective, KeyPerformanceIndicators, Stakeholders, Timeframe, OrganizationalVisionAlignment, Risks, Dependencies, UserCreated, DateCreated)
VALUES 
('Increase digital banking adoption', 'Grow digital-first customer base by 30% over 18 months', '["Mobile app enrollment rate","Digital transaction volume","Customer satisfaction score"]', '["Digital Banking VP","Marketing Director","Customer Experience Team"]', CONVERT(DATE, '2025-12-31'), 'Aligns with our vision to be the leading digital-first bank in the region', '["Competitive offerings","Technology adoption barriers"]', '["Mobile app redesign project","Digital marketing campaign"]', 'admin', GETDATE()),
('Enhance fraud prevention', 'Reduce fraud losses by 25% while maintaining positive customer experience', '["Fraud detection rate","False positive rate","Fraud loss percentage"]', '["Security Officer","Fraud Department","Risk Management"]', CONVERT(DATE, '2025-09-30'), 'Supports our commitment to secure banking', '["Evolving fraud patterns","Customer friction"]', '["AI detection system","Staff training"]', 'admin', GETDATE()),
('Expand commercial lending', 'Increase business loan portfolio by 20% focusing on small and medium businesses', '["Loan origination volume","Market share percentage","Portfolio yield"]', '["Commercial Banking VP","Credit Risk Manager","Branch Managers"]', CONVERT(DATE, '2025-12-31'), 'Supports our goal to be the preferred financial partner', '["Economic downturn","Regulatory changes"]', '["Credit scoring enhancement","Loan officer training"]', 'admin', GETDATE()),
('Optimize branch network', 'Consolidate physical locations while improving customer service metrics', '["Cost per transaction","Customer satisfaction","Branch utilization rate"]', '["Retail Banking Director","Operations Head","Facilities Manager"]', CONVERT(DATE, '2025-06-30'), 'Balances digital transformation with personalized service', '["Customer backlash","Competitor advantage"]', '["Digital banking enhancement","Staff reallocation plan"]', 'admin', GETDATE());

-- 2. OperationModel (4 records)
INSERT INTO OperationModels (OperationalProcess, ProcessOwner, Inputs, Outputs, KeyActivities, ResourcesRequired, PerformanceMetrics, Challenges, UserCreated, DateCreated)
VALUES 
('Loan Origination Process', 'Credit Operations Director', '["Loan applications","Customer documentation","Credit reports"]', '["Loan approval decision","Loan documents","Funding instructions"]', '["Application receipt","Document verification","Credit analysis","Underwriting","Approval/decline","Loan document generation"]', '["Loan processing staff","Credit analysts","Underwriting system","Document management system"]', '["Time to decision","Approval rate","Documentation quality"]', '["Manual document verification","Legacy system integration"]', 'admin', GETDATE()),
('Customer Onboarding', 'Customer Experience Manager', '["Customer identification","KYC documentation","Account opening forms"]', '["New customer account","Welcome package","Online banking credentials"]', '["Identity verification","Background checks","Account setup","Card issuance","Online access provision"]', '["Branch staff","Compliance team","Account processing system","Card issuance system"]', '["Onboarding time","First transaction time","Error rate"]', '["Regulatory compliance","System integration issues"]', 'admin', GETDATE()),
('Payment Processing', 'Payment Operations Head', '["Payment instructions","Account information","Authentication data"]', '["Completed transactions","Transaction records","Settlement reports"]', '["Payment validation","Fraud screening","Processing","Settlement","Notification"]', '["Payment processors","Fraud detection system","Settlement system","Notification system"]', '["Transaction success rate","Processing time","Fraud detection accuracy"]', '["High volume during peak times","System downtime"]', 'admin', GETDATE()),
('Dispute Resolution', 'Customer Service Manager', '["Customer complaint","Transaction details","Account history"]', '["Resolution outcome","Customer communication","Account adjustment"]', '["Case logging","Investigation","Customer communication","Resolution determination","Account adjustment"]', '["Service representatives","Case management system","Investigation team","Adjustment authorization"]', '["Resolution time","Customer satisfaction","First-contact resolution rate"]', '["Documentation gathering","Multiple touchpoints"]', 'admin', GETDATE());

-- 3. OrganizationView (4 records)
INSERT INTO OrganizationViews (DepartmentOrUnit, RolesAndResponsibilities, ReportingStructure, KeyPersonnel, CollaborationPoints, OrganizationalGoals, Challenges, UserCreated, DateCreated)
VALUES 
('Retail Banking', 'Oversees consumer banking products, branch operations, and direct customer service', 'Reports to Chief Banking Officer, with Branch Network, Product Management, and Customer Service reporting to department head', '["Sarah Johnson (EVP Retail Banking)","Michael Chen (Branch Network Director)","Priya Patel (Product Development Manager)"]', '["Digital Banking","Operations","Marketing","Risk Management"]', '["Increase customer retention","Grow deposit base","Improve customer satisfaction"]', '["Digital transformation","Branch profitability","Staff turnover"]', 'admin', GETDATE()),
('Commercial Banking', 'Manages business lending, commercial accounts, and business advisory services', 'Reports to Chief Banking Officer, with Lending, Treasury Services, and Relationship Management as sub-units', '["Thomas Williams (SVP Commercial Banking)","Jamal Washington (Head of Business Lending)","Lisa Rodriguez (Treasury Services Director)"]', '["Credit Risk","Capital Markets","Retail Banking","Legal"]', '["Grow loan portfolio","Increase fee income","Expand market share"]', '["Competitive marketplace","Credit quality","Regulatory burden"]', 'admin', GETDATE()),
('Risk Management', 'Responsible for enterprise risk framework, credit risk assessment, and compliance assurance', 'Reports to Chief Risk Officer, with Credit Risk, Operational Risk, and Compliance as sub-departments', '["Robert Chan (Enterprise Risk Director)","Elizabeth Miller (Credit Risk Head)","James Norton (Compliance Officer)"]', '["All Lending Units","Treasury","Audit","Legal"]', '["Maintain credit quality","Ensure regulatory compliance","Reduce operational losses"]', '["Evolving regulations","Risk data quality","Process standardization"]', 'admin', GETDATE()),
('Information Technology', 'Delivers and maintains technology services, infrastructure, and applications', 'Reports to Chief Information Officer, with Application Development, Infrastructure, and IT Security as key groups', '["David Singh (CIO)","Maria Gonzalez (Application Development Head)","Kevin Lee (Infrastructure Manager)"]', '["All Business Units","Risk Management","Operations","Vendors"]', '["System availability","Project delivery","Cost efficiency"]', '["Legacy systems","Resource constraints","Technology changes"]', 'admin', GETDATE());

-- 4. CapabilityMap (4 records)
INSERT INTO CapabilityMaps (CapabilityName, CapabilityDescription, MaturityLevel, Owner, Dependencies, KeyProcesses, Gaps, UserCreated, DateCreated)
VALUES 
('Credit Risk Assessment', 'Capability to evaluate, quantify and manage credit risk for all loan products', 'Advanced', 'Credit Risk Director', '["Data quality management","Regulatory compliance","Market intelligence"]', '["Application scoring","Portfolio analysis","Stress testing","Limit management"]', '["Real-time scoring","Alternative data integration"]', 'admin', GETDATE()),
('Digital Customer Onboarding', 'Capability to enroll new customers entirely through digital channels', 'Intermediate', 'Digital Banking Head', '["Identity verification","Document management","Customer communication"]', '["Identity verification","Document upload","Account creation","Card issuance"]', '["Biometric verification","Straight-through processing"]', 'admin', GETDATE()),
('Treasury Management', 'Capability to manage bank liquidity, investments, and balance sheet optimization', 'Advanced', 'Treasury Director', '["Market data analysis","Regulatory reporting","Risk modeling"]', '["Cash position monitoring","Investment management","Asset-liability matching","Regulatory reporting"]', '["Algorithmic trading","Enhanced forecasting"]', 'admin', GETDATE()),
('Payment Processing', 'Capability to execute domestic and international payment transactions', 'Mature', 'Payment Operations Manager', '["Fraud detection","Compliance checking","Correspondent banking"]', '["Transaction validation","Routing determination","Settlement","Reconciliation"]', '["Real-time cross-border payments","Blockchain integration"]', 'admin', GETDATE());

-- 5. ProcessModel (4 records) - will add complex properties later
INSERT INTO ProcessModels (ProcessName, ProcessDescription, Inputs, Outputs, StepsWorkflow, Owner, ToolsSystemsUsed, PerformanceMetrics, CustomerType, LeadTime, Bottlenecks, ImprovementOpportunities, MappingDate, UserCreated, DateCreated)
VALUES 
('Mortgage Loan Origination', 'End-to-end process for mortgage application, approval, and closing', '["Loan application","Property information","Customer financials","Credit reports"]', '["Loan decision","Mortgage documents","Funding instructions"]', '["Application receipt","Initial review","Property appraisal","Underwriting","Decision","Closing preparation","Loan closing"]', 'Mortgage Director', '["Loan Origination System","Document Management System","Credit Bureau Interface","Property Valuation System"]', '["Time to decision","Approval rate","Customer satisfaction","Pull-through rate"]', 'Retail Customers', '', '["Manual document review","Underwriting capacity","Third-party appraisals"]', '["Electronic document capture","Automated underwriting expansion","Process parallelization"]', CONVERT(DATE, '2024-12-15'), 'admin', GETDATE()),
('Account Opening Process', 'Process to establish new customer relationships and accounts', '["Customer identification","KYC documents","Product selection","Initial deposit"]', '["Account establishment","Access credentials","Welcome materials"]', '["Identity verification","Background checking","Account setup","Card issuance","Online access provision"]', 'Customer Operations Manager', '["Core Banking System","KYC System","Card Management System","Digital Banking Platform"]', '["Onboarding time","Error rate","Account activation rate"]', 'All Customers', '', '["Identity verification","System integrations","Manual reviews"]', '["Digital ID verification","Pre-filled applications","Process automation"]', CONVERT(DATE, '2024-11-20'), 'admin', GETDATE()),
('Business Loan Approval', 'Process for reviewing, approving, and funding business loans', '["Loan application","Business financials","Business plan","Collateral information"]', '["Credit decision","Loan documents","Funding"]', '["Application receipt","Initial screening","Financial analysis","Risk assessment","Approval committee","Documentation","Funding"]', 'Commercial Credit Director', '["Commercial Loan System","Financial Analysis Tools","Credit Committee Workflow","Document Generation System"]', '["Time to decision","Risk-adjusted return","Documentation accuracy"]', 'Business Customers', '', '["Financial analysis","Committee scheduling","Documentation complexity"]', '["Financial data integration","Committee process optimization","Template standardization"]', CONVERT(DATE, '2024-10-05'), 'admin', GETDATE()),
('Fraud Claim Resolution', 'Process to investigate and resolve customer fraud claims', '["Fraud report","Transaction details","Customer statement","Evidence"]', '["Investigation outcome","Account adjustment","Customer notification"]', '["Claim receipt","Initial assessment","Evidence collection","Investigation","Determination","Account correction","Customer notification"]', 'Fraud Operations Manager', '["Case Management System","Transaction Database","Investigation Tools","Customer Communication System"]', '["Resolution time","Customer satisfaction","Recovery rate","Regulatory compliance"]', 'All Customers', '', '["Evidence collection","Third-party responses","Complex cases"]', '["Automated evidence gathering","Case prioritization","Self-service tracking"]', CONVERT(DATE, '2024-09-10'), 'admin', GETDATE());

-- 6. BusinessProductServiceView (4 records)
INSERT INTO BusinessProductServiceViews (ProductServiceName, Description, TargetMarketCustomer, ValueProposition, RevenueModel, KeyFeatures, Dependencies, Competitors, UserCreated, DateCreated)
VALUES 
('Premier Checking Account', 'Full-featured checking account with premium benefits for high-value customers', 'Affluent individuals with minimum $10,000 average balance', 'Comprehensive banking solution with personalized service and exclusive benefits', 'Account fees, interchange revenue, balance-based earnings', '["No monthly fee with balance","Priority customer service","Preferential rates","Free premium services"]', '["Core banking system","Digital banking platform","Card processing system"]', '["National Bank Gold Account","City Premium Banking","Regional Premier Account"]', 'admin', GETDATE()),
('Small Business Lending Program', 'Specialized loan products designed for small businesses', 'Small businesses with annual revenue under $5 million', 'Fast, accessible financing with simplified application process and relationship-based approach', 'Interest income, origination fees, servicing fees', '["Streamlined application","Quick decisions","Relationship pricing","Flexible terms"]', '["Commercial loan system","Credit scoring model","Document management system"]', '["Capital Small Business Loans","First Business Bank","Online Lenders"]', 'admin', GETDATE()),
('Mobile Banking Platform', 'Comprehensive banking application for smartphones and tablets', 'All retail and small business customers', 'Complete banking functionality with convenient, secure access anywhere, anytime', 'Indirect through customer acquisition/retention, transaction fees, marketing platform', '["Account management","Bill payment","Mobile deposit","P2P transfers","Card controls"]', '["Core banking integration","Security infrastructure","API gateway"]', '["MegaBank Mobile","FinTech Banking Apps","Digital-Only Banks"]', 'admin', GETDATE()),
('Wealth Management Advisory', 'Personalized investment advisory and financial planning services', 'High net worth individuals with $250,000+ investable assets', 'Expert financial guidance with comprehensive planning and customized investment strategies', 'Advisory fees, asset-based fees, product commissions', '["Dedicated advisor","Custom portfolio construction","Financial planning","Tax optimization"]', '["Investment platform","Financial planning tools","CRM system"]', '["Investment Bank Wealth","Advisor Group","Robo-Advisors"]', 'admin', GETDATE());

-- ==========================================
-- APPLICATION ARCHITECTURE DOMAIN - 6 Artifact Types
-- ==========================================

-- 1. ApplicationArchitectureFramework (4 records) - will add complex properties later
INSERT INTO ApplicationArchitectureFrameworks (ApplicationName, ApplicationDescription, BusinessFunctionSupported, TechnologyStack, IntegrationPoints, Owner, Version, DeploymentEnvironment, RationalizationCategory, RationalizationRationale, BusinessValue, TechnicalFit, AnnualCost, RiskScore, CurrentState, FutureState, RedundantWith, RedundantApplications, ReplacementOptions, AssessmentDate, AssessedBy, UserCreated, DateCreated, Vendor)
VALUES 
('Core Banking System', 'Primary system of record for all accounts, customers, and transactions', 'Account Management, Transaction Processing, Customer Information', '["Java","Oracle Database","Linux","Apache Tomcat"]', '["Digital Banking","Loan Origination","Card Management","Reporting System"]', 'Banking Systems Director', '3.8.2', 'Production', 'Invest', 'Critical system supporting all banking operations', 8, 7, 1500000.00, 9, 'Stable but aging core modules', 'Modular architecture with API-first approach', '[]', '[]', '["Modern Core Platforms","Cloud-Native Banking"]', CONVERT(DATE, '2024-10-15'), 'IT Architecture Team', 'admin', GETDATE(), 'CoreBank Solutions'),
('Digital Banking Platform', 'Omni-channel platform for customer self-service banking', 'Online Banking, Mobile Banking, Customer Self-Service', '["Angular","Node.js","MongoDB","AWS"]', '["Core Banking","Payment Gateway","Authentication Service","Card Management"]', 'Digital Channels Manager', '4.2.0', 'Production', 'Invest', 'Strategic digital engagement platform with growing usage', 9, 8, 800000.00, 6, 'Modern platform with regular updates', 'Enhanced personalization and expanded capabilities', '[]', '[]', '[]', CONVERT(DATE, '2024-10-10'), 'Digital Banking Team', 'admin', GETDATE(), 'Digital Engage Tech'),
('Loan Origination System', 'End-to-end system for processing and approving loan applications', 'Loan Application Processing, Underwriting, Document Management', '["C#",".NET Framework","SQL Server","Windows Server"]', '["Core Banking","Document Management","Credit Bureau","Pricing Engine"]', 'Credit Systems Manager', '2.5.4', 'Production', 'Migrate', 'Functional but limited scalability and integration capabilities', 7, 5, 350000.00, 7, 'Stable but difficult to enhance', 'Cloud-based solution with greater automation', '[]', '[]', '["Modern Lending Platforms","SaaS Lending Solutions"]', CONVERT(DATE, '2024-09-25'), 'Lending Technology Team', 'admin', GETDATE(), 'LendPro Systems'),
('Customer Relationship Management', 'System for managing customer interactions and relationships', 'Sales Management, Customer Service, Marketing Campaigns', '["Salesforce","AWS","Heroku"]', '["Core Banking","Marketing Automation","Call Center","Data Warehouse"]', 'Customer Experience Director', '2023.4', 'Production', 'Invest', 'Strategic platform for customer-centric initiatives', 8, 9, 600000.00, 4, 'Modern SaaS implementation', 'Expanded analytics and AI-driven insights', '[]', '[]', '[]', CONVERT(DATE, '2024-10-05'), 'CRM Working Group', 'admin', GETDATE(), 'Salesforce');

-- 2. ApplicationBusinessRequirementsView (4 records)
INSERT INTO ApplicationBusinessRequirementsViews (RequirementId, RequirementDescription, Priority, Stakeholders, Status, AssignedTo, DueDate, Dependencies, UserCreated, DateCreated)
VALUES 
('REQ-COR-001', 'Core Banking System must support real-time transaction processing with 99.99% uptime', 'High', '["Operations Director","Digital Banking Head","Retail Banking Lead"]', 'Approved', 'Banking Systems Director', CONVERT(DATE, '2025-06-30'), '["Infrastructure upgrade","High-availability architecture"]', 'admin', GETDATE()),
('REQ-DIG-002', 'Digital Banking Platform must support biometric authentication across all mobile devices', 'High', '["Digital Banking Head","Security Officer","Customer Experience Lead"]', 'In Progress', 'Digital Channels Manager', CONVERT(DATE, '2025-03-15'), '["Identity verification service","Mobile app update"]', 'admin', GETDATE()),
('REQ-LOS-003', 'Loan Origination System must provide straight-through processing for pre-approved personal loans', 'Medium', '["Consumer Lending Director","Risk Officer","Digital Banking Head"]', 'Draft', 'Lending Systems Analyst', CONVERT(DATE, '2025-05-31'), '["Credit scoring model update","Digital banking integration"]', 'admin', GETDATE()),
('REQ-CRM-004', 'CRM must provide a unified 360-degree customer view integrating data from all banking systems', 'High', '["Customer Experience Director","Marketing Head","Branch Operations"]', 'Approved', 'CRM Program Manager', CONVERT(DATE, '2025-04-15'), '["Data warehouse integration","API development","Data quality initiative"]', 'admin', GETDATE());

-- 3. ServiceCatalogue (4 records)
INSERT INTO ServiceCatalogues (ServiceName, ServiceDescription, ServiceOwner, SLA, Cost, SupportedApplications, Availability, Dependencies, UserCreated, DateCreated)
VALUES 
('Customer Authentication Service', 'Centralized service for validating customer identity across all channels', 'Security Services Manager', '99.99% uptime, <500ms response time', 120000.00, '["Digital Banking Platform","Mobile Banking App","ATM Network","Branch Teller System"]', '24x7 with planned maintenance windows', '["Identity management system","Biometric verification service","Fraud detection system"]', 'admin', GETDATE()),
('Payment Processing Service', 'Service for executing payment transactions across multiple rails', 'Payment Systems Director', '99.95% uptime, <1s transaction time', 250000.00, '["Core Banking System","Digital Banking Platform","Mobile Banking App","ATM Network"]', '24x7', '["Core banking system","Payment networks","Settlement systems"]', 'admin', GETDATE()),
('Account Information Service', 'Service providing customer account details and transaction history', 'Banking Systems Manager', '99.9% uptime, <800ms response time', 95000.00, '["Digital Banking Platform","Mobile Banking App","Customer Service Portal","ATM Network"]', '24x7 with planned maintenance windows', '["Core banking system","Transaction database","Data warehouse"]', 'admin', GETDATE()),
('Document Management Service', 'Service for storing, retrieving, and managing customer documents', 'Enterprise Content Manager', '99.5% uptime, <2s retrieval time', 85000.00, '["Loan Origination System","Account Opening System","Customer Service Portal","Compliance System"]', 'Business hours with extended support', '["Document repository","Content management system","Workflow engine"]', 'admin', GETDATE());

-- 4. ApplicationToApplicationCrossReference (4 records)
INSERT INTO ApplicationToApplicationCrossReferences (SourceApplication, TargetApplication, IntegrationType, DataExchanged, Frequency, Owner, SecurityRequirements, Dependencies, UserCreated, DateCreated)
VALUES 
('Digital Banking Platform', 'Core Banking System', 'REST API', 'Account information, transaction data, customer profile data', 'Real-time', 'Integration Services Manager', 'TLS 1.3 encryption, OAuth 2.0 authentication, API key validation', '["API gateway","Authentication service","Logging service"]', 'admin', GETDATE()),
('Loan Origination System', 'Core Banking System', 'Web Services', 'Customer information, account details, loan disbursement instructions', 'Real-time and batch', 'Lending Systems Manager', 'TLS 1.2 encryption, service account authentication, IP whitelisting', '["Integration middleware","Data validation service"]', 'admin', GETDATE()),
('CRM System', 'Core Banking System', 'REST API and Batch ETL', 'Customer profiles, relationship data, account summaries, interaction history', 'Real-time and daily batch', 'CRM Systems Manager', 'TLS 1.3 encryption, service account authentication, data masking for sensitive fields', '["API gateway","ETL platform","Data warehouse"]', 'admin', GETDATE()),
('Mobile Banking App', 'Digital Banking Platform', 'REST API', 'Authentication, account information, transaction execution, notifications', 'Real-time', 'Mobile Banking Manager', 'TLS 1.3 encryption, OAuth 2.0 with MFA, certificate pinning', '["API gateway","Authentication service","Push notification service"]', 'admin', GETDATE());

-- 5. ApplicationSecurityModel (4 records)
INSERT INTO ApplicationSecurityModels (ApplicationName, SecurityRequirement, ComplianceStandards, Vulnerabilities, MitigationStrategies, Owner, LastAuditDate, Dependencies, UserCreated, DateCreated)
VALUES 
('Core Banking System', 'Must protect customer financial data with comprehensive security controls', '["PCI-DSS","GLBA","SOX"]', '["Legacy authentication mechanisms","Outdated encryption standards in older modules","Incomplete logging"]', '["Enhanced authentication with MFA","Encryption upgrade project","Comprehensive logging implementation"]', 'Security Officer', CONVERT(DATE, '2024-09-15'), '["Identity management upgrade","Encryption services","Security monitoring system"]', 'admin', GETDATE()),
('Digital Banking Platform', 'Must secure all customer interactions and prevent unauthorized access', '["PCI-DSS","GDPR","FFIEC"]', '["Session management weaknesses","Mobile API vulnerabilities","Third-party component risks"]', '["Session security enhancement","API security gateway","Dependency scanning automation"]', 'Digital Security Manager', CONVERT(DATE, '2024-10-10'), '["API gateway security","Mobile security framework","Vulnerability scanning tools"]', 'admin', GETDATE()),
('Loan Origination System', 'Must protect sensitive customer financial and personal information', '["GLBA","FCRA","SOX"]', '["Insufficient data encryption at rest","Role-based access control gaps","Audit logging deficiencies"]', '["Database encryption implementation","Access control review and enhancement","Audit logging expansion"]', 'Lending Systems Security Lead', CONVERT(DATE, '2024-08-05'), '["Encryption service","Identity management system","Security monitoring tool"]', 'admin', GETDATE()),
('CRM System', 'Must protect customer personal information and maintain confidentiality', '["GLBA","GDPR","CCPA"]', '["Data export controls","User privilege management","Third-party access"]', '["Data loss prevention implementation","Role review and cleanup","Partner access framework"]', 'CRM Security Administrator', CONVERT(DATE, '2024-09-25'), '["DLP system","Identity governance","Partner management system"]', 'admin', GETDATE());

-- 6. ApplicationDataCrossReference (4 records)
INSERT INTO ApplicationDataCrossReferences (ApplicationName, DataEntity, DataUsage, DataSensitivity, AccessControls, Owner, DataRetentionPolicy, Dependencies, UserCreated, DateCreated)
VALUES 
('Core Banking System', 'Customer Master Data', 'System of record for all customer identity and profile information', 'High - Contains PII and KYC data', 'Role-based access control with specific entitlements for customer data access', 'Data Management Officer', '7 years after account closure in accordance with retention policy', '["Data governance framework","Identity management system","Auditing system"]', 'admin', GETDATE()),
('Digital Banking Platform', 'Transaction Data', 'Displays and processes customer financial transactions', 'High - Contains financial transaction details', 'Encrypted transmission, role-based access, and customer authentication', 'Digital Banking Data Owner', 'Transaction data retained for 7 years in accordance with banking regulations', '["Core banking integration","Encryption services","Authentication system"]', 'admin', GETDATE()),
('Loan Origination System', 'Loan Application Data', 'Captures and processes customer loan applications and supporting documents', 'High - Contains financial and personal information', 'Department-based access with additional role restrictions for sensitive information', 'Lending Data Steward', 'Approved applications: 7 years after payoff; Denied applications: 25 months', '["Document management system","Access control system","Archive system"]', 'admin', GETDATE()),
('CRM System', 'Customer Interaction Data', 'Records all customer touchpoints and interaction history', 'Medium - Contains contact history and preferences', 'Role-based access with department-level segregation', 'Customer Experience Data Owner', '5 years rolling retention with annual archiving', '["Core banking integration","Communication systems","Data warehouse"]', 'admin', GETDATE());

-- ==========================================
-- DATA ARCHITECTURE DOMAIN - 6 Artifact Types
-- ==========================================

-- 1. DataGovernanceModel (4 records)
INSERT INTO DataGovernanceModels (DataEntity, DataOwner, DataSteward, DataQualityMetrics, DataPolicies, ComplianceRequirements, DataLifecycle, Dependencies, UserCreated, DateCreated)
VALUES 
('Customer Master Data', 'Chief Data Officer', 'Customer Data Steward', '["Completeness rate","Accuracy rate","Uniqueness score","Consistency measure"]', '["Customer data must be validated at source","Changes to core attributes require approval","PII data must be encrypted"]', '["GDPR","GLBA","KYC regulations","CIP requirements"]', 'Creation at onboarding, regular updates through multiple channels, archival after account closure + retention period', '["Data quality tools","Customer onboarding systems","Data integration platform"]', 'admin', GETDATE()),
('Account Master Data', 'Finance Data Director', 'Account Data Steward', '["Accuracy rate","Completeness score","Validity measure","Timeliness metric"]', '["Account data changes require dual approval","Historical data must be preserved","Account status changes must be logged"]', '["SOX","GAAP reporting","Regulatory reporting requirements"]', 'Creation at account opening, regular updates through transactions and maintenance, archival after closure + retention period', '["Core banking system","General ledger","Reporting systems"]', 'admin', GETDATE()),
('Transaction Data', 'Operations Data Director', 'Transaction Data Steward', '["Accuracy score","Completeness rate","Consistency measure","Timeliness metric"]', '["All transactions must have complete audit trail","Transaction modifications require special privileges","Transactions cannot be deleted"]', '["BSA/AML requirements","PCI-DSS","GLBA","SOX"]', 'Creation at transaction time, immutable after posting, archival after retention period', '["Transaction processing systems","Fraud monitoring tools","Archive systems"]', 'admin', GETDATE()),
('Product Catalog Data', 'Product Management Head', 'Product Data Steward', '["Completeness score","Consistency measure","Accuracy rate","Approval status"]', '["Product data changes require business approval","Product terms must be version controlled","Product data requires legal review"]', '["Truth in lending","UDAAP compliance","Disclosure requirements"]', 'Creation during product development, regular updates through product lifecycle, archival when products retired', '["Product management system","Document management","Marketing systems"]', 'admin', GETDATE());

-- 2. InformationHierarchyView (4 records)
INSERT INTO InformationHierarchyViews (InformationLevel, ParentInformation, ChildInformation, Description, Owner, Usage, Dependencies, SecurityLevel, UserCreated, DateCreated)
VALUES 
('Enterprise', NULL, '["Customer Domain","Account Domain","Transaction Domain","Product Domain"]', 'Top-level organization of bank information domains', 'Chief Data Officer', 'Provides enterprise-wide information organization and governance structure', '["Enterprise data model","Data governance framework","Master data management"]', 'Internal Only', 'admin', GETDATE()),
('Domain - Customer', 'Enterprise', '["Customer Profile","Customer Relationships","Customer Preferences","Customer Documents"]', 'Organization of all customer-related information', 'Customer Data Director', 'Supports customer management, service, and analytics', '["Customer master data","CRM system","KYC system"]', 'Confidential', 'admin', GETDATE()),
('Domain - Account', 'Enterprise', '["Deposit Accounts","Loan Accounts","Investment Accounts","Credit Card Accounts"]', 'Organization of all account-related information', 'Account Data Director', 'Supports account management, reporting, and analytics', '["Account master data","Product catalog","General ledger"]', 'Confidential', 'admin', GETDATE()),
('Subject Area - Deposit Accounts', 'Domain - Account', '["Checking Accounts","Savings Accounts","CDs","Money Market Accounts"]', 'Organization of deposit account information', 'Retail Banking Data Owner', 'Supports deposit product management and reporting', '["Core banking system","Deposit product catalog","Interest calculation systems"]', 'Confidential', 'admin', GETDATE());

-- 3. InformationRequirementsView (4 records)
INSERT INTO InformationRequirementsViews (RequirementId, RequirementDescription, DataEntity, Priority, Stakeholders, Status, AssignedTo, DueDate, UserCreated, DateCreated)
VALUES 
('IR-CUST-001', 'Need consolidated customer contact history across all channels to support 360-degree customer view', 'Customer Interaction Data', 'High', '["Customer Experience Director","Marketing Manager","Branch Operations"]', 'In Progress', 'CRM Data Architect', CONVERT(DATE, '2025-03-31'), 'admin', GETDATE()),
('IR-RISK-002', 'Need integrated customer risk profile combining credit history, transaction patterns, and relationship data', 'Customer Risk Profile', 'High', '["Risk Management Head","Fraud Department","Compliance Officer"]', 'Approved', 'Risk Data Architect', CONVERT(DATE, '2025-06-30'), 'admin', GETDATE()),
('IR-FIN-003', 'Need product profitability analysis incorporating direct costs, allocated expenses, and risk adjustments', 'Product Performance Data', 'Medium', '["Finance Director","Product Management","Executive Team"]', 'Draft', 'Financial Data Analyst', CONVERT(DATE, '2025-04-15'), 'admin', GETDATE()),
('IR-COMP-004', 'Need automated suspicious activity detection using consolidated transaction data across products', 'Transaction Monitoring Data', 'High', '["Compliance Officer","AML Team","Risk Management"]', 'Approved', 'Compliance Data Specialist', CONVERT(DATE, '2025-02-28'), 'admin', GETDATE());

-- 4. DataFlowModel (4 records)
INSERT INTO DataFlowModels (DataSource, DataDestination, DataFlowDescription, DataTypeName, Frequency, Owner, SecurityRequirements, Dependencies, UserCreated, DateCreated)
VALUES 
('Core Banking System', 'Data Warehouse', 'Comprehensive extract of customer accounts, balances, and transactions for analytics and reporting', 'Core Banking Data', 'Daily full load, hourly incremental', 'Data Integration Manager', 'Encryption in transit and at rest, access limited to data team', '["ETL platform","Network infrastructure","Storage system"]', 'admin', GETDATE()),
('Digital Banking Platform', 'Customer Analytics Platform', 'Customer digital behavior including login patterns, feature usage, and journey data', 'Digital Interaction Data', 'Real-time streaming', 'Digital Analytics Manager', 'Data anonymization for sensitive fields, encrypted transmission', '["Streaming platform","API gateway","Analytics system"]', 'admin', GETDATE()),
('Loan Origination System', 'Credit Risk System', 'Application data, credit decisions, and loan terms for risk assessment and modeling', 'Loan Data', 'Near real-time event-based', 'Credit Risk Data Manager', 'Strict access controls, field-level encryption for sensitive data', '["Messaging system","Data transformation service","Authentication system"]', 'admin', GETDATE()),
('Card Management System', 'Fraud Detection Platform', 'Card transaction details for real-time fraud analysis and prevention', 'Card Transaction Data', 'Real-time streaming', 'Fraud Analytics Manager', 'Secure transmission, comprehensive data protection, minimal data retention', '["Streaming platform","Network infrastructure","Security monitoring"]', 'admin', GETDATE());

-- 5. LogicalDataModel (4 records)
INSERT INTO LogicalDataModels (EntityName, Attributes, Relationships, PrimaryKey, ForeignKey, Owner, Description, Dependencies, UserCreated, DateCreated)
VALUES 
('Customer', '["CustomerID","FirstName","LastName","DateOfBirth","TaxID","Address","Phone","Email","Status"]', '["Customer has many Accounts","Customer has many Documents","Customer belongs to Segments"]', 'CustomerID', NULL, 'Customer Data Architect', 'Core entity representing individual or business customers of the bank', '["Customer onboarding process","KYC process","Customer master data management"]', 'admin', GETDATE()),
('Account', '["AccountID","AccountNumber","CustomerID","ProductID","Status","OpenDate","CloseDate","Balance","Currency"]', '["Account belongs to Customer","Account has many Transactions","Account has one Product"]', 'AccountID', 'CustomerID, ProductID', 'Account Data Architect', 'Financial account held by a customer, associated with a specific banking product', '["Account opening process","Core banking system","General ledger"]', 'admin', GETDATE()),
('Transaction', '["TransactionID","AccountID","TransactionDate","Amount","TransactionType","Description","Status","ReferenceNumber"]', '["Transaction belongs to Account","Transaction has one TransactionType"]', 'TransactionID', 'AccountID', 'Transaction Data Architect', 'Financial transaction affecting an account balance', '["Payment processing","Accounting system","Transaction monitoring"]', 'admin', GETDATE()),
('Product', '["ProductID","ProductCode","ProductName","ProductType","Description","Status","LaunchDate","RetirementDate"]', '["Product has many Accounts","Product has many Features","Product has many Fees"]', 'ProductID', NULL, 'Product Data Architect', 'Banking product offered to customers, such as checking account or loan type', '["Product management system","Pricing system","Document management"]', 'admin', GETDATE());

-- 6. DataSecurityModel (4 records)
INSERT INTO DataSecurityModels (DataEntity, SecurityRequirement, AccessControls, EncryptionRequirements, ComplianceStandards, Owner, LastAuditDate, Dependencies, UserCreated, DateCreated)
VALUES 
('Customer Personal Data', 'Protect all personally identifiable information from unauthorized access and disclosure', 'Role-based access with attribute-based restrictions and privileged access management', 'Encryption at rest using AES-256, encryption in transit using TLS 1.3', '["GDPR","GLBA","CCPA","Internal data protection policy"]', 'Data Security Officer', CONVERT(DATE, '2024-09-15'), '["Identity management system","Encryption platform","Access control system"]', 'admin', GETDATE()),
('Payment Card Data', 'Secure cardholder data according to industry standards and prevent unauthorized access', 'Strict role-based access with separation of duties and just-in-time privileged access', 'Encryption at rest, tokenization for storage, point-to-point encryption for transmission', '["PCI-DSS","Card network rules","Internal security policy"]', 'Card Security Manager', CONVERT(DATE, '2024-08-10'), '["Tokenization system","HSM infrastructure","Key management system"]', 'admin', GETDATE()),
('Financial Transaction Data', 'Ensure transaction integrity, confidentiality, and non-repudiation', 'Function-based access control with transaction amount limits and segregation of duties', 'Encryption in transit, digitally signed transaction records, secure storage', '["SOX","GLBA","BSA/AML requirements"]', 'Transaction Security Manager', CONVERT(DATE, '2024-10-05'), '["Digital signature system","Audit logging platform","Monitoring tools"]', 'admin', GETDATE()),
('Authentication Credentials', 'Protect authentication data from compromise or unauthorized use', 'Highly restricted access limited to authentication system processes', 'One-way hashing with strong algorithms, salting, and key strengthening', '["FFIEC Authentication Guidance","NIST standards","Internal security policy"]', 'Identity Security Manager', CONVERT(DATE, '2024-07-20'), '["Identity management system","Credential management","Security monitoring"]', 'admin', GETDATE());

-- ==========================================
-- INFRASTRUCTURE ARCHITECTURE DOMAIN - 6 Artifact Types
-- ==========================================

-- 1. InfrastructureBusinessRequirementsView (4 records)
INSERT INTO InfrastructureBusinessRequirementsViews (RequirementId, RequirementDescription, BusinessUnit, Priority, Stakeholders, Status, AssignedTo, DueDate, UserCreated, DateCreated)
VALUES 
('INFRA-DR-001', 'Implement enhanced disaster recovery capabilities with RPO < 4 hours and RTO < 8 hours for all critical systems', 'Enterprise Risk Management', 'High', '["CIO","Business Continuity Manager","Operations Director"]', 'Approved', 'Infrastructure Resilience Manager', CONVERT(DATE, '2025-06-30'), 'admin', GETDATE()),
('INFRA-CLOUD-002', 'Establish hybrid cloud infrastructure to support digital banking platform with 99.99% availability', 'Digital Banking', 'High', '["Digital Banking Head","CTO","Security Officer"]', 'In Progress', 'Cloud Architecture Manager', CONVERT(DATE, '2025-03-31'), 'admin', GETDATE()),
('INFRA-NET-003', 'Implement network segmentation for payment processing systems to enhance security and compliance', 'Payment Operations', 'High', '["CISO","Payment Operations Head","Compliance Officer"]', 'Approved', 'Network Security Manager', CONVERT(DATE, '2025-04-15'), 'admin', GETDATE()),
('INFRA-PERF-004', 'Enhance core banking infrastructure to support 30% transaction volume increase during peak periods', 'Retail Banking', 'Medium', '["Retail Banking Head","Operations Director","Core Banking Manager"]', 'Draft', 'Infrastructure Performance Lead', CONVERT(DATE, '2025-08-31'), 'admin', GETDATE());

-- 2. SystemToApplicationCrossReference (4 records)
INSERT INTO SystemToApplicationCrossReferences (SystemName, ApplicationName, IntegrationType, DataExchanged, Frequency, Owner, SecurityRequirements, Dependencies, UserCreated, DateCreated)
VALUES 
('Production Database Cluster', 'Core Banking System', 'Direct Database Connection', 'All core banking data including customer, account, and transaction records', 'Continuous', 'Database Operations Manager', 'TDE encryption, network isolation, privileged access management', '["Database management system","Storage infrastructure","Backup system"]', 'admin', GETDATE()),
('Web Application Servers', 'Digital Banking Platform', 'Container Orchestration', 'Web application services, API endpoints, customer session data', 'Continuous', 'Web Infrastructure Manager', 'TLS encryption, WAF protection, container security controls', '["Container platform","Load balancers","Security monitoring"]', 'admin', GETDATE()),
('Payment Processing Network', 'Payment Gateway', 'Dedicated Secure Network', 'Payment instructions, authorization messages, settlement data', 'Continuous', 'Network Operations Manager', 'Encrypted connections, network segmentation, intrusion detection', '["Network infrastructure","Security devices","Monitoring systems"]', 'admin', GETDATE()),
('Data Warehouse Cluster', 'Enterprise Reporting System', 'ETL Pipelines', 'Consolidated business data for analytics and reporting', 'Scheduled batch and real-time streaming', 'Analytics Infrastructure Manager', 'Data encryption, access controls, secure ETL processes', '["Data warehouse platform","ETL tools","Storage systems"]', 'admin', GETDATE());

-- 3. ResourceNeedsModel (4 records)
INSERT INTO ResourceNeedsModels (ResourceType, ResourceName, Description, Quantity, Owner, Cost, Dependencies, AllocationStatus, UserCreated, DateCreated)
VALUES 
('Server Hardware', 'Database Server Cluster', 'High-performance servers for core banking database deployment', 12, 'Infrastructure Director', 480000.00, '["Data center facility","Network infrastructure","System software"]', 'Allocated', 'admin', GETDATE()),
('Cloud Service', 'Secure Cloud Environment', 'Private cloud infrastructure for digital banking platform', 1, 'Cloud Operations Manager', 350000.00, '["Network connectivity","Security controls","Identity management"]', 'In Use', 'admin', GETDATE()),
('Network Equipment', 'Next-Gen Firewall Suite', 'Advanced security appliances for perimeter and internal segmentation', 8, 'Network Security Manager', 240000.00, '["Network infrastructure","Security monitoring","Management platform"]', 'Allocated', 'admin', GETDATE()),
('Software License', 'Database Management System', 'Enterprise license for core database platform', 1, 'Database Operations Manager', 620000.00, '["Server hardware","Operating system","Support contract"]', 'In Use', 'admin', GETDATE());

-- 4. SystemToBusinessCrossReference (4 records)
INSERT INTO SystemToBusinessCrossReferences (SystemName, BusinessUnit, BusinessProcessSupported, Owner, Criticality, Dependencies, PerformanceMetrics, SecurityRequirements, UserCreated, DateCreated)
VALUES 
('Core Banking Infrastructure', 'Retail Banking', 'Customer account management, transaction processing, and balance maintenance', 'Banking Systems Director', 'Critical', '["Data center facilities","Database system","Network connectivity"]', '["System availability","Transaction response time","Batch processing completion"]', 'Comprehensive security controls including encryption, access management, and monitoring', 'admin', GETDATE()),
('Digital Banking Platform Infrastructure', 'Digital Banking', 'Online and mobile banking services, digital customer engagement', 'Digital Infrastructure Manager', 'Critical', '["Cloud services","Application servers","Security components"]', '["Platform availability","Response time","User capacity"]', 'Advanced security including WAF, anti-fraud measures, and encryption', 'admin', GETDATE()),
('Payment Processing Infrastructure', 'Treasury Services', 'Electronic payment processing, wire transfers, ACH operations', 'Payment Infrastructure Manager', 'Critical', '["Network connectivity","Processing servers","Security components"]', '["Transaction throughput","Processing time","Success rate"]', 'PCI-DSS compliant environment with network segmentation and encryption', 'admin', GETDATE()),
('Lending Platform Infrastructure', 'Consumer Lending', 'Loan origination, servicing, and portfolio management', 'Lending Systems Manager', 'High', '["Application servers","Database systems","Document management"]', '["System availability","Application response time","Processing capacity"]', 'Secure environment with data protection and access controls', 'admin', GETDATE());

-- 5. InfrastructureSecurityModel (4 records)
INSERT INTO InfrastructureSecurityModels (SystemName, SecurityRequirement, ComplianceStandards, Vulnerabilities, MitigationStrategies, Owner, LastAuditDate, Dependencies, UserCreated, DateCreated)
VALUES 
('Core Banking Network', 'Secure and isolate core banking systems from unauthorized access and cyber threats', '["PCI-DSS","FFIEC","ISO 27001"]', '["Legacy system integration points","Complex access requirements","Aging network equipment"]', '["Network segmentation implementation","Advanced threat protection","Access control enhancement"]', 'Network Security Director', CONVERT(DATE, '2024-08-15'), '["Security monitoring platform","Network management tools","Compliance verification system"]', 'admin', GETDATE()),
('Cloud Infrastructure', 'Ensure secure cloud environment with appropriate controls and monitoring', '["SOC 2","PCI-DSS","Cloud Security Alliance"]', '["Misconfiguration risks","Shared responsibility gaps","Identity management complexity"]', '["Cloud security posture management","Enhanced IAM controls","Cloud-native security tools"]', 'Cloud Security Manager', CONVERT(DATE, '2024-09-05'), '["Cloud management platform","Identity system","Security monitoring"]', 'admin', GETDATE()),
('End-User Computing', 'Protect all endpoints from malware and unauthorized access', '["NIST Cybersecurity Framework","ISO 27001","Internal policy"]', '["Phishing vulnerability","Unpatched systems","Unauthorized software"]', '["Advanced endpoint protection","Regular patching","Application control"]', 'Endpoint Security Manager', CONVERT(DATE, '2024-07-20'), '["Endpoint management system","Patch management","Security awareness program"]', 'admin', GETDATE()),
('Data Center Infrastructure', 'Ensure physical and environmental security of critical computing resources', '["ISO 27001","Uptime Institute","FFIEC"]', '["Physical access controls","Environmental monitoring gaps","Power resilience"]', '["Enhanced access control","Improved monitoring","Power redundancy"]', 'Data Center Security Manager', CONVERT(DATE, '2024-10-10'), '["Access control system","Environmental monitoring","Power systems"]', 'admin', GETDATE());

-- 6. SystemDataCrossReference (4 records)
INSERT INTO SystemDataCrossReferences (SystemName, DataEntity, DataUsage, DataSensitivity, AccessControls, Owner, DataRetentionPolicy, Dependencies, UserCreated, DateCreated)
VALUES 
('Core Banking Servers', 'Customer Financial Data', 'Storage and processing of all customer accounts and transaction information', 'High', 'Role-based access control with privilege management and monitoring', 'Database Security Manager', 'Active data maintained online, older data archived according to retention schedule', '["Database encryption","Access management system","Audit logging"]', 'admin', GETDATE()),
('Digital Banking Servers', 'Customer Online Banking Data', 'Processing and temporary storage of customer online banking activities', 'High', 'Strict access control with temporary session data and minimal persistence', 'Digital Security Manager', 'Transient data purged after session, logs retained according to security policy', '["Session management","Encryption system","Security monitoring"]', 'admin', GETDATE()),
('Data Warehouse Systems', 'Consolidated Reporting Data', 'Storage and analysis of business and customer data for reporting and analytics', 'Medium-High', 'Role-based access with data masking and usage tracking', 'Data Analytics Security Manager', 'Data maintained according to business need and regulatory requirements', '["Data masking tools","Access management","Audit system"]', 'admin', GETDATE()),
('Document Management Systems', 'Customer Documentation', 'Storage and retrieval of customer-provided documents and generated statements', 'High', 'Document-level access control with encryption and activity monitoring', 'Content Security Manager', 'Documents maintained according to document type-specific retention schedules', '["Document encryption","Access control system","Archive system"]', 'admin', GETDATE());

-- ==========================================
-- Add Complex Properties for ProcessModel and ApplicationArchitectureFramework
-- ==========================================

-- First, retrieve the IDs for proper foreign key relationships
-- Process Model IDs
DECLARE @ProcessModel1ID int = (SELECT Id FROM ProcessModels WHERE ProcessName = 'Mortgage Loan Origination');
DECLARE @ProcessModel2ID int = (SELECT Id FROM ProcessModels WHERE ProcessName = 'Account Opening Process');
DECLARE @ProcessModel3ID int = (SELECT Id FROM ProcessModels WHERE ProcessName = 'Business Loan Approval');
DECLARE @ProcessModel4ID int = (SELECT Id FROM ProcessModels WHERE ProcessName = 'Fraud Claim Resolution');

-- Application Architecture Framework IDs
DECLARE @AppFramework1ID int = (SELECT Id FROM ApplicationArchitectureFrameworks WHERE ApplicationName = 'Core Banking System');
DECLARE @AppFramework2ID int = (SELECT Id FROM ApplicationArchitectureFrameworks WHERE ApplicationName = 'Digital Banking Platform');
DECLARE @AppFramework3ID int = (SELECT Id FROM ApplicationArchitectureFrameworks WHERE ApplicationName = 'Loan Origination System');
DECLARE @AppFramework4ID int = (SELECT Id FROM ApplicationArchitectureFrameworks WHERE ApplicationName = 'Customer Relationship Management');

-- Process Stages for ProcessModel 1 (Mortgage Loan Origination)
INSERT INTO ProcessStages (Id, Name, Description, ProcessTime, WaitTime, ProcessIds, ApplicationIds, IsValueAdd, PercentComplete, DefectRate, Issues, ProcessModelId)
VALUES 
(NEWID(), 'Application Receipt', 'Customer submits mortgage application via branch or online channel', '', '', '["APP-001","APP-002"]', '["LOS-1","DMS-1"]', 1, 100.0, 0.02, '["Incomplete applications","System downtime"]', @ProcessModel1ID),
(NEWID(), 'Initial Review', 'Loan officer reviews application for completeness and basic qualification', '', '', '["APP-003"]', '["LOS-1"]', 1, 100.0, 0.05, '["Missing documentation","Staff availability"]', @ProcessModel1ID),
(NEWID(), 'Property Appraisal', 'Third-party conducts property valuation', '', '', '["APP-004"]', '["LOS-1","APS-1"]', 1, 100.0, 0.08, '["Appraiser availability","Scheduling delays"]', @ProcessModel1ID),
(NEWID(), 'Underwriting', 'Credit analysis and risk assessment', '', '', '["APP-005"]', '["LOS-1","CRS-1"]', 1, 100.0, 0.03, '["Complex cases","Workload distribution"]', @ProcessModel1ID),
(NEWID(), 'Decision', 'Final approval or decline decision', '', '', '["APP-006"]', '["LOS-1","WFL-1"]', 1, 100.0, 0.01, '["Exception handling","Approval authority"]', @ProcessModel1ID);

-- Process Stages for ProcessModel 2 (Account Opening Process)
INSERT INTO ProcessStages (Id, Name, Description, ProcessTime, WaitTime, ProcessIds, ApplicationIds, IsValueAdd, PercentComplete, DefectRate, Issues, ProcessModelId)
VALUES 
(NEWID(), 'Identity Verification', 'Verify customer identity through documentation and checks', '', '', '["AOP-001"]', '["CBS-1","KYC-1"]', 1, 100.0, 0.03, '["ID document issues","System response time"]', @ProcessModel2ID),
(NEWID(), 'Background Checking', 'Perform regulatory and compliance checks', '', '', '["AOP-002"]', '["KYC-1","CMP-1"]', 1, 100.0, 0.02, '["False positives","External system availability"]', @ProcessModel2ID),
(NEWID(), 'Account Setup', 'Configure and create customer account in system', '', '', '["AOP-003"]', '["CBS-1"]', 1, 100.0, 0.01, '["System performance","Product selection complexity"]', @ProcessModel2ID),
(NEWID(), 'Card Issuance', 'Order and provision payment cards', '', '', '["AOP-004"]', '["CBS-1","CRD-1"]', 0, 100.0, 0.04, '["Card production delays","Delivery issues"]', @ProcessModel2ID);

-- Process Stages for ProcessModel 3 (Business Loan Approval)
INSERT INTO ProcessStages (Id, Name, Description, ProcessTime, WaitTime, ProcessIds, ApplicationIds, IsValueAdd, PercentComplete, DefectRate, Issues, ProcessModelId)
VALUES 
(NEWID(), 'Application Receipt', 'Receive and log business loan application', '', '', '["BLA-001"]', '["LOS-1","DMS-1"]', 1, 100.0, 0.02, '["Incomplete submissions","Application tracking"]', @ProcessModel3ID),
(NEWID(), 'Initial Screening', 'Review application for basic qualification criteria', '', '', '["BLA-002"]', '["LOS-1"]', 1, 100.0, 0.03, '["Staff availability","Screening criteria clarity"]', @ProcessModel3ID),
(NEWID(), 'Financial Analysis', 'Detailed analysis of business financials', '', '', '["BLA-003"]', '["LOS-1","FAS-1"]', 1, 100.0, 0.05, '["Financial data quality","Analysis complexity"]', @ProcessModel3ID),
(NEWID(), 'Risk Assessment', 'Evaluate business and credit risk factors', '', '', '["BLA-004"]', '["LOS-1","CRS-1"]', 1, 100.0, 0.04, '["Risk model limitations","Industry-specific factors"]', @ProcessModel3ID);

-- Stages for ProcessModel 4 (Fraud Claim Resolution)
INSERT INTO ProcessStages (Id, Name, Description, ProcessTime, WaitTime, ProcessIds, ApplicationIds, IsValueAdd, PercentComplete, DefectRate, Issues, ProcessModelId)
VALUES 
(NEWID(), 'Claim Receipt', 'Receive and log customer fraud claim', '', '', '["FCR-001"]', '["CMS-1","DMS-1"]', 1, 100.0, 0.02, '["Information gathering","Channel limitations"]', @ProcessModel4ID),
(NEWID(), 'Initial Assessment', 'Preliminary review of claim validity', '', '', '["FCR-002"]', '["CMS-1","FDS-1"]', 1, 100.0, 0.05, '["Case complexity","Data availability"]', @ProcessModel4ID),
(NEWID(), 'Investigation', 'Detailed investigation of transactions and evidence', '', '', '["FCR-003"]', '["CMS-1","FDS-1"]', 1, 100.0, 0.04, '["Investigation complexity","External party responsiveness"]', @ProcessModel4ID),
(NEWID(), 'Determination', 'Decision on claim validity and resolution approach', '', '', '["FCR-004"]', '["CMS-1","WFL-1"]', 1, 100.0, 0.03, '["Decision authority","Procedure exceptions"]', @ProcessModel4ID);

-- Add StakeholderValueItems for ProcessModels
INSERT INTO StakeholderValues (Id, StakeholderType, ValueDescription, Priority, ProcessId, MetricOfSuccess, ProcessModelId)
VALUES 
-- For ProcessModel 1 (Mortgage Loan Origination)
(NEWID(), 'Customer', 'Fast and transparent mortgage application process', 9, 'MP-001', 'Time to decision < 10 days', @ProcessModel1ID),
(NEWID(), 'Loan Officer', 'Efficient workflow with minimal manual steps', 7, 'MP-001', 'Applications processed per day > 5', @ProcessModel1ID),
(NEWID(), 'Risk Management', 'Consistent underwriting with appropriate risk controls', 8, 'MP-001', 'Loan default rate < industry average', @ProcessModel1ID),

-- For ProcessModel 2 (Account Opening Process)
(NEWID(), 'Customer', 'Quick and hassle-free account opening experience', 9, 'AO-001', 'Account opening time < 30 minutes', @ProcessModel2ID),
(NEWID(), 'Branch Staff', 'Streamlined process with minimal errors or rework', 8, 'AO-001', 'First-time-right rate > 95%', @ProcessModel2ID),
(NEWID(), 'Compliance', 'All regulatory requirements properly fulfilled', 10, 'AO-001', 'Compliance exceptions < 0.1%', @ProcessModel2ID),

-- For ProcessModel 3 (Business Loan Approval)
(NEWID(), 'Business Customer', 'Clear requirements and quick decision process', 9, 'BL-001', 'Time to decision < 7 days', @ProcessModel3ID),
(NEWID(), 'Relationship Manager', 'Transparent process status and decision rationale', 7, 'BL-001', 'Customer satisfaction score > 4.5/5', @ProcessModel3ID),
(NEWID(), 'Credit Committee', 'Complete information package for decision-making', 8, 'BL-001', 'Cases requiring additional information < 10%', @ProcessModel3ID),

-- For ProcessModel 4 (Fraud Claim Resolution)
(NEWID(), 'Customer', 'Quick resolution of fraud claims with minimal burden', 10, 'FC-001', 'Claim resolution time < 5 days', @ProcessModel4ID),
(NEWID(), 'Fraud Department', 'Sufficient information to properly investigate claims', 8, 'FC-001', 'Case reopening rate < 5%', @ProcessModel4ID),
(NEWID(), 'Compliance', 'All regulatory requirements for fraud handling met', 9, 'FC-001', 'Regulatory finding rate < 0.5%', @ProcessModel4ID);

-- Add ApplicationRecommendations for ApplicationArchitectureFramework
INSERT INTO ApplicationRecommendations (Id, ApplicationId, RecommendationType, Description, Business, EstimatedCostSavings, EstimatedImplementationCost, Timeline, Dependencies, Status, ApplicationFrameworkId)
VALUES 
-- For AppFramework 1 (Core Banking System)
(NEWID(), '1', 'Invest', 'Upgrade to latest version with improved API capabilities', 'All business units', 250000.00, 950000.00, '12 months', '["Infrastructure upgrade","Data migration","Integration testing"]', 'Approved', @AppFramework1ID),
(NEWID(), '1', 'Enhance', 'Implement real-time payment processing module', 'Retail Banking, Commercial Banking', 350000.00, 600000.00, '9 months', '["Payment gateway integration","Security enhancement","Compliance review"]', 'Pending', @AppFramework1ID),

-- For AppFramework 2 (Digital Banking Platform)
(NEWID(), '2', 'Invest', 'Enhance mobile app with biometric authentication', 'Digital Banking', 120000.00, 300000.00, '6 months', '["Mobile security framework","Biometric integration","User acceptance testing"]', 'In Progress', @AppFramework2ID),
(NEWID(), '2', 'Enhance', 'Implement personalization engine for tailored customer experience', 'Retail Banking, Marketing', 450000.00, 550000.00, '8 months', '["Customer analytics platform","UX redesign","A/B testing framework"]', 'Approved', @AppFramework2ID),

-- For AppFramework 3 (Loan Origination System)
(NEWID(), '3', 'Replace', 'Migrate to cloud-based lending platform', 'Consumer Lending, Commercial Lending', 650000.00, 1200000.00, '18 months', '["Data migration","Integration development","Process redesign","User training"]', 'Pending', @AppFramework3ID),
(NEWID(), '3', 'Enhance', 'Implement automated underwriting for consumer loans', 'Consumer Lending', 400000.00, 450000.00, '9 months', '["Risk model development","Business rule engine","Integration testing"]', 'Approved', @AppFramework3ID),

-- For AppFramework 4 (CRM)
(NEWID(), '4', 'Invest', 'Expand analytics capabilities with machine learning models', 'Marketing, Customer Experience', 300000.00, 400000.00, '10 months', '["Data science platform","Model development","Integration work"]', 'In Progress', @AppFramework4ID),
(NEWID(), '4', 'Enhance', 'Implement omnichannel engagement capabilities', 'All business units', 250000.00, 350000.00, '7 months', '["Integration with communication platforms","Business process adjustments","User training"]', 'Approved', @AppFramework4ID);

-- ==========================================
-- Create Artifact Links between domains
-- ==========================================

INSERT INTO ArtifactLinks (Id, SourceArtifactType, SourceArtifactId, TargetArtifactType, TargetArtifactId, LinkDescription, LinkCreatedDate, CreatedBy)
VALUES 
-- Business Strategy to Process
(NEWID(), 0, '1', 4, '1', 'Digital banking adoption strategy supports mortgage loan origination process', GETDATE(), 'admin'),
(NEWID(), 0, '3', 4, '3', 'Commercial lending strategy drives the business loan approval process', GETDATE(), 'admin'),
(NEWID(), 0, '2', 4, '4', 'Fraud prevention strategy underpins the fraud claim resolution process', GETDATE(), 'admin'),

-- Business Strategy to Application Framework
(NEWID(), 0, '1', 6, '2', 'Digital banking adoption strategy is implemented through the Digital Banking Platform', GETDATE(), 'admin'),
(NEWID(), 0, '3', 6, '3', 'Commercial lending strategy is enabled by the Loan Origination System', GETDATE(), 'admin'),
(NEWID(), 0, '2', 6, '4', 'Fraud prevention strategy leverages the CRM System for customer interaction tracking', GETDATE(), 'admin'),

-- Process to Application Framework
(NEWID(), 4, '1', 6, '3', 'Mortgage loan origination process is supported by the Loan Origination System', GETDATE(), 'admin'),
(NEWID(), 4, '2', 6, '1', 'Account opening process is supported by the Core Banking System', GETDATE(), 'admin'),
(NEWID(), 4, '3', 6, '3', 'Business loan approval process is supported by the Loan Origination System', GETDATE(), 'admin'),

-- Organization to Capability
(NEWID(), 2, '1', 3, '2', 'Retail Banking department owns the Digital Customer Onboarding capability', GETDATE(), 'admin'),
(NEWID(), 2, '2', 3, '1', 'Commercial Banking department relies on the Credit Risk Assessment capability', GETDATE(), 'admin'),
(NEWID(), 2, '3', 3, '1', 'Risk Management department oversees the Credit Risk Assessment capability', GETDATE(), 'admin'),
(NEWID(), 2, '4', 3, '4', 'Information Technology department supports the Payment Processing capability', GETDATE(), 'admin'),

-- Application Framework to Data Entity
(NEWID(), 6, '1', 12, '1', 'Core Banking System manages Customer Master Data', GETDATE(), 'admin'),
(NEWID(), 6, '1', 12, '2', 'Core Banking System manages Account Master Data', GETDATE(), 'admin'),
(NEWID(), 6, '2', 12, '3', 'Digital Banking Platform processes Transaction Data', GETDATE(), 'admin'),
(NEWID(), 6, '3', 12, '4', 'Loan Origination System uses Product Catalog Data', GETDATE(), 'admin'),

-- Business Product to Application
(NEWID(), 5, '1', 6, '1', 'Premier Checking Account product is managed in the Core Banking System', GETDATE(), 'admin'),
(NEWID(), 5, '2', 6, '3', 'Small Business Lending Program is supported by the Loan Origination System', GETDATE(), 'admin'),
(NEWID(), 5, '3', 6, '2', 'Mobile Banking Platform product is provided by the Digital Banking Platform', GETDATE(), 'admin'),
(NEWID(), 5, '4', 6, '4', 'Wealth Management Advisory is supported by the CRM System', GETDATE(), 'admin'),

-- Application to Infrastructure
(NEWID(), 6, '1', 20, '1', 'Core Banking System runs on Production Database Cluster', GETDATE(), 'admin'),
(NEWID(), 6, '2', 20, '2', 'Digital Banking Platform runs on Web Application Servers', GETDATE(), 'admin'),
(NEWID(), 6, '3', 20, '4', 'Loan Origination System runs on Lending Platform Infrastructure', GETDATE(), 'admin'),

-- Process to Data Flow
(NEWID(), 4, '1', 15, '3', 'Mortgage Loan Origination process generates data flow from Loan Origination System to Credit Risk System', GETDATE(), 'admin'),
(NEWID(), 4, '4', 15, '4', 'Fraud Claim Resolution process uses data flow from Card Management System to Fraud Detection Platform', GETDATE(), 'admin'),

-- Data Security to Infrastructure Security
(NEWID(), 17, '1', 22, '1', 'Customer Personal Data security requirements are implemented in Core Banking Network security', GETDATE(), 'admin'),
(NEWID(), 17, '2', 22, '1', 'Payment Card Data security is enforced by Core Banking Network security', GETDATE(), 'admin'),
(NEWID(), 17, '4', 22, '3', 'Authentication Credentials security is managed by End-User Computing security', GETDATE(), 'admin'),

-- Cross-domain connections for complete traceability
(NEWID(), 0, '1', 15, '2', 'Digital banking adoption strategy relies on Digital Banking Platform to Customer Analytics Platform data flow', GETDATE(), 'admin'),
(NEWID(), 4, '1', 23, '4', 'Mortgage Loan Origination process stores documents in Document Management Systems', GETDATE(), 'admin'),
(NEWID(), 3, '1', 20, '1', 'Credit Risk Assessment capability is supported by Production Database Cluster', GETDATE(), 'admin'),
(NEWID(), 6, '2', 15, '2', 'Digital Banking Platform sends data to Customer Analytics Platform', GETDATE(), 'admin'),
(NEWID(), 6, '4', 23, '3', 'CRM System relies on Data Warehouse Systems for reporting', GETDATE(), 'admin');